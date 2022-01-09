using Outlook = Microsoft.Office.Interop.Outlook;
using System;

namespace CheckMyMail
{
    public class MailRecipient : IComparable
    {
        public MailRecipient(string type, string address, string domain,
                         string help, bool isSMTP)
        {
            Type = type;
            Address = address;
            Domain = domain;
            Help = help;
            IsSMTP = isSMTP;
        }

        public string Type { get; }
        public string Address { get; }
        public string Domain { get; }
        public string Help { get; }
        public bool IsSMTP { get; }

        public int CompareTo(object other)
        {
            if (other == null) return 1;

            MailRecipient omr = other as MailRecipient;
            /*
             * Non-SMTP addresses come first.
             */
            if (IsSMTP && !omr.IsSMTP)
                return 1;
            if (!IsSMTP && omr.IsSMTP)
                return -1;

            /*
             * Sort by domain. This is the crux that essentially
             * makes MainDialog.RenderAddressList() work.
             */
            var ret = String.Compare(Domain, omr.Domain);
            if (ret != 0)
            {
                return ret;
            }

            /* Sort by recipient types (To > Cc > Bcc) */
            ret = String.Compare(Type, omr.Type);
            if (ret != 0)
            {
                return -ret;
            }
            return String.Compare(Address, omr.Address);
        }
    }

    class MailRecipientFactory
    {
        static public MailRecipient Create(Outlook.Recipient recp)
        {
            string Type = GetType(recp);
            string Address;
            string Domain;
            string Help;
            bool IsSMTP = CheckSMTP(recp);

            /*
             * First, we process normal SMTP entries. This is the easiest
             * part, because they are just addresses like "foo@example.com".
             */
            if (IsSMTP)
            {
                Address = recp.Address;
                Help = Address;
                Domain = Address.Substring(Address.IndexOf('@') + 1);
                return new MailRecipient(Type, Address, Domain, Help, IsSMTP);
            }

            /*
             * Next we need to handle the non-SMTP recipients. For details, see:
             * https://docs.microsoft.com/en-us/dotnet/api/microsoft.office.interop.outlook.oldisplaytype
             */
            switch (recp.AddressEntry.DisplayType)
            {
                case Outlook.OlDisplayType.olUser:
                    Domain = "Exchange";
                    break;
                case Outlook.OlDisplayType.olRemoteUser:
                    Domain = "Exchange（外部）";
                    break;
                case Outlook.OlDisplayType.olDistList:
                case Outlook.OlDisplayType.olPrivateDistList:
                    Domain = "送信先リスト";
                    break;
                default:
                    Domain = "その他";
                    break;
            }
            Address = recp.Name;
            Help = $"[{Domain}] {recp.Name}";
            return new MailRecipient(Type, Address, Domain, Help, IsSMTP);
        }

        private static bool CheckSMTP(Outlook.Recipient recp)
        {
            if (recp.AddressEntry.DisplayType == Outlook.OlDisplayType.olUser)
            {
                return recp.AddressEntry.Type == "SMTP";
            }
            return false;
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
    }
}