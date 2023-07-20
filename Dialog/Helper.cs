using System;
using System.Text.RegularExpressions;
using Outlook = Microsoft.Office.Interop.Outlook;

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

        public RecipientInfo(Outlook.Recipient recp)
        {
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
                FromSMTP(recp);
            }
            else if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olUser ||
                     recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olRemoteUser)
            {
                FromExchange(recp);
            }
            else if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olPrivateDistList ||
                     recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olDistList)
            {
                FromDistList(recp);
            }
            else
            {
                FromOther(recp);
            }
        }

        private void FromSMTP(Outlook.Recipient recp)
        {
            QueueLogger.Log(" => FromSMTP");
            Type = GetType(recp);
            Address = recp.Address;
            Domain = GetDomainFromSMTP(Address);
            Help = Address;
            IsSMTP = true;
        }

        private void FromExchange(Outlook.Recipient recp)
        {
            QueueLogger.Log(" => FromExchange");
            Outlook.ExchangeUser user = recp.AddressEntry.GetExchangeUser();
            QueueLogger.Log($"  user: {user}");

            string possibleAddress = "";
            if (user == null ||
                string.IsNullOrEmpty(user.PrimarySmtpAddress))
            {
                QueueLogger.Log("  user is null or has no PrimarySmtpAddress: trying to get it via PropertyAccessor");
                const string PR_SMTP_ADDRESS = "https://schemas.microsoft.com/mapi/proptag/0x39FE001E";
                possibleAddress = GetSMTPAddressViaAccessor(recp, PR_SMTP_ADDRESS);
                if (string.IsNullOrEmpty(possibleAddress))
                {
                    const string PR_EMS_PROXY_ADDRESSES = "http://schemas.microsoft.com/mapi/proptag/0x800f101e";
                    possibleAddress = GetSMTPAddressViaAccessor(recp, PR_EMS_PROXY_ADDRESSES);
                }
            }
            else
            {
                possibleAddress = user.PrimarySmtpAddress;
            }

            if (string.IsNullOrEmpty(possibleAddress))
            {
                QueueLogger.Log("  Couldn't get address: fallback to FromOther");
                FromOther(recp);
                return;
            }
            QueueLogger.Log($"  => finally resolved addrss: {possibleAddress}");

            Type = GetType(recp);
            Address = possibleAddress;
            Domain = GetDomainFromSMTP(Address);
            Help = Address;
            IsSMTP = true;
        }

        private string GetSMTPAddressViaAccessor(Outlook.Recipient recp, string schemaName)
        {
            try
            {
                QueueLogger.Log($"  Retrieving values for {schemaName}...");
                object propertyValue = recp.AddressEntry.PropertyAccessor.GetProperty(schemaName);
                if (propertyValue is string[] values)
                {
                    foreach (string value in values)
                    {
                        QueueLogger.Log($"  value: {value}");
                        // The recipient may have multiple values with their types like:
                        //   SIP:local@domain
                        //   SMTP:local@domain
                        // We should accept only SMTP address.
                        if (!string.IsNullOrEmpty(value) &&
                            Regex.IsMatch(value, "^(SMTP:)?[^:@]+@.+", RegexOptions.IgnoreCase))
                        {
                            return Regex.Replace(value, "^SMTP:", "");
                        }
                    }
                }
                return propertyValue.ToString();
            }
            catch (Exception ex)
            {
                QueueLogger.Log($"  Failed to GetProperty with {schemaName}: {ex}");
                return "";
            }
        }

        private void FromDistList(Outlook.Recipient recp)
        {
            QueueLogger.Log(" => FromDistList");
            Outlook.ExchangeDistributionList dist = recp.AddressEntry.GetExchangeDistributionList();
            QueueLogger.Log($"  dist: {dist}");
            if (dist == null || string.IsNullOrEmpty(dist.PrimarySmtpAddress))
            {
                FromOther(recp);
            }
            else
            {
                Type = GetType(recp);
                Address = dist.PrimarySmtpAddress;
                Domain = GetDomainFromSMTP(Address);
                Help = Address;
                IsSMTP = true;
            }
        }

        private void FromOther(Outlook.Recipient recp)
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

            Type = GetType(recp);
            Address = recp.Name;
            Help = $"[{Domain}] {Address}";
            IsSMTP = false;
        }

        private string GetDomainFromSMTP(string addr)
        {
            return addr.Substring(addr.IndexOf('@') + 1).ToLower();
        }

        private static string GetType(Outlook.Recipient recp)
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
