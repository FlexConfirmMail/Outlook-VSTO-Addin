using System;
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
        public const string DOMAIN_EXCHANGE_EXT = "Exchange (外部)";

        public RecipientInfo(Outlook.Recipient recp)
        {
            if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olUser
                && recp.AddressEntry.Type == "SMTP")
            {
                FromSMTP(recp);
            }
            else
            {
                FromOther(recp);
            }
        }

        private void FromSMTP(Outlook.Recipient recp)
        {
            Type = GetType(recp);
            Address = recp.Address;
            Domain = Address.Substring(Address.IndexOf('@') + 1);
            Help = Address;
            IsSMTP = true;
        }

        private void FromOther(Outlook.Recipient recp)
        {
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

            /*
             * Non-SMTP addresses come first.
             */
            if (IsSMTP && !info.IsSMTP)
                return 1;
            if (!IsSMTP && info.IsSMTP)
                return -1;

            /*
             * Sort by domain. This is the crux that essentially
             * makes MainDialog.RenderAddressList() work.
             */
            var ret = String.Compare(Domain, info.Domain);
            if (ret != 0)
            {
                return ret;
            }

            /* Sort by recipient types (To > Cc > Bcc) */
            ret = String.Compare(Type, info.Type);
            if (ret != 0)
            {
                return -ret;
            }
            return String.Compare(Address, info.Address);
        }
    }
}
