using System;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Azure.Identity;

namespace FlexConfirmMail.Dialog
{
    internal class RecipientInfo : IComparable
    {
        public string Type { get; set; }
        public string Address { get; set; }
        public string Domain { get; set; }
        public string Help { get; set; }
        public bool IsSMTP { get; set; }

        public const string DOMAIN_EXCHANGE = "Exchange";
        public const string DOMAIN_EXCHANGE_EXT = "Exchange (ext)";

        private static Outlook.Recipient recp { get; set; }

        public RecipientInfo(Outlook.Recipient recipient)
        {
            recp = recipient;

            recp.Resolve();
            QueueLogger.Log("RecipientInfo");
            QueueLogger.Log($"  Resolved: {recp.Resolved}");
            QueueLogger.Log($"  Name: {recp.Name}");
            QueueLogger.Log($"  Type: {recp.Type}");
            QueueLogger.Log($"  Address: {recp.Address}");
            QueueLogger.Log($"  AddressEntry.Name: {recp.AddressEntry.Name}");
            QueueLogger.Log($"  AddressEntry.Address: {recp.AddressEntry.Address}");
            QueueLogger.Log($"  AddressEntry.DisplayType: {recp.AddressEntry.DisplayType}");
            QueueLogger.Log($"  AddressEntry.Type: {recp.AddressEntry.Type}");

            if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olUser
                && recp.AddressEntry.Type == "SMTP")
            {
                FromSMTP();
            }
            else if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olUser ||
                     recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olRemoteUser)
            {
                FromExchange();
            }
            else if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olPrivateDistList ||
                     recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olDistList)
            {
                FromDistList();
            }
            else
            {
                FromOther();
            }
        }

        private void FromSMTP()
        {
            QueueLogger.Log(" => FromSMTP");
            Type = GetType();
            Address = recp.Address;
            Domain = GetDomainFromSMTP(Address);
            Help = Address;
            IsSMTP = true;
        }

        private void FromExchange()
        {
          FromExchangeAsync().Wait();
        }
        private async Task<bool> FromExchangeAsync()
        {
            QueueLogger.Log(" => FromExchange");
            Outlook.ExchangeUser user = recp.AddressEntry.GetExchangeUser();
            QueueLogger.Log($"  user: {user}");
            if (user == null)
            {
                FromOther();
                return true;
            }

            string PossibleAddress = user.PrimarySmtpAddress;
            if (string.IsNullOrEmpty(PossibleAddress))
            {
                QueueLogger.Log("  PrimarySmtpAddress is blank: trying to get it via Microsoft Graph API");
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //var scopes = new[] { "User.Read" };
                //var options = new DeviceCodeCredentialOptions
                //{
                //    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                //    ClientId = "<UUID>",
                //    TenantId = "common"
                //};
                //var credential = new DeviceCodeCredential(options);
                //var graphClient = new GraphServiceClient(credential, scopes);
                var ClientId = "<UUID>";
                var TenantId = "common";
                var ClientSecret = "<secret>";
                var scopes = new[] {"https://graph.microsoft.com/.default"};
                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                };
                var credential = new ClientSecretCredential(TenantId, ClientId, ClientSecret, options);
                var graphClient = new GraphServiceClient(credential, scopes);
                try
                {
                QueueLogger.Log($"  user.ID: {user.ID}");
                var result = await graphClient.Users[user.ID].GetAsync().ConfigureAwait(false);
                QueueLogger.Log($"  result: {result}");
                PossibleAddress = result.UserPrincipalName;
                if (string.IsNullOrEmpty(PossibleAddress))
                {
                    QueueLogger.Log("  Couldn't get address");
                    FromOther();
                    return true;
                }
                }
                catch(Microsoft.Graph.Models.ODataErrors.ODataError e)
                {
                    QueueLogger.Log($"  error: code={e.Error.Code}, message={e.Error.Message}, details={e.Error.Details}");
                }
            }
            QueueLogger.Log($"  => finally resolved addrss: {PossibleAddress}");

            Type = GetType();
            Address = PossibleAddress;
            Domain = GetDomainFromSMTP(Address);
            Help = Address;
            IsSMTP = true;

            return true;
        }

        private void FromDistList()
        {
            QueueLogger.Log(" => FromDistList");
            Outlook.ExchangeDistributionList dist = recp.AddressEntry.GetExchangeDistributionList();
            QueueLogger.Log($"  dist: {dist}");
            if (dist == null || string.IsNullOrEmpty(dist.PrimarySmtpAddress))
            {
                FromOther();
            }
            else
            {
                Type = GetType();
                Address = dist.PrimarySmtpAddress;
                Domain = GetDomainFromSMTP(Address);
                Help = Address;
                IsSMTP = true;
            }
        }

        private void FromOther()
        {
            QueueLogger.Log($" => FromOther ({recp.AddressEntry.DisplayType})");
            switch (recp.AddressEntry.DisplayType)
            {
                case Outlook.OlDisplayType.olUser:
                    Domain = DOMAIN_EXCHANGE;
                    break;
                case Outlook.OlDisplayType.olRemoteUser:
                    Domain = DOMAIN_EXCHANGE_EXT;
                    break;
                case Outlook.OlDisplayType.olDistList:
                case Outlook.OlDisplayType.olPrivateDistList:
                    Domain = "送信先リスト";
                    break;
                default:
                    Domain = "その他";
                    break;
            }

            Type = GetType();
            Address = recp.Name;
            Help = $"[{Domain}] {Address}";
            IsSMTP = false;
        }

        private string GetDomainFromSMTP(string addr)
        {
            return addr.Substring(addr.IndexOf('@') + 1).ToLower();
        }

        private static string GetType()
        {
            switch (recp.Type)
            {
                case (int)Outlook.OlMailRecipientType.olBCC:
                    return "Bcc";
                case (int)Outlook.OlMailRecipientType.olCC:
                    return "Cc";
                default:
                    return "To";
            }
        }

        public int CompareTo(object other)
        {
            if (other == null)
            {
                return 1;
            }
            RecipientInfo info = other as RecipientInfo;

            // Non-SMTP addresses come first.
            if (IsSMTP && !info.IsSMTP)
                return 1;
            if (!IsSMTP && info.IsSMTP)
                return -1;

            // Sort by domain. This is the crux that essentially
            // makes MainDialog.RenderAddressList() work.
            var ret = String.Compare(Domain, info.Domain);
            if (ret != 0)
            {
                return ret;
            }

            // Sort by recipient types (To > Cc > Bcc)
            ret = String.Compare(Type, info.Type);
            if (ret != 0)
            {
                return -ret;
            }
            return String.Compare(Address, info.Address);
        }
    }
}
