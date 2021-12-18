using Outlook = Microsoft.Office.Interop.Outlook;

namespace CheckMyMail
{
    public class MailRecipient
    {
        public Outlook.Recipient Recipient { get; }
        public string Group { get; }
        public string Address { get; }
        public string RecipientType { get; }
        public string Tooltip { get; }
        public int DispOrder { get; }

        public MailRecipient(Outlook.Recipient recp)
        {
            Recipient = recp;
            RecipientType = GetSendType(recp);
            Address = recp.Name;
            DispOrder = 0;

            switch (recp.AddressEntry.DisplayType)
            {
                case Outlook.OlDisplayType.olUser:
                    if (recp.AddressEntry.Type == "SMTP")
                    {
                        Group = GetDomain(recp.Address);
                        Address = recp.Address;
                        Tooltip = Address;
                        DispOrder = 1;
                    }
                    else
                    {
                        Group = "Exchange";
                        Tooltip = $"[{Group}] {Address}";
                    }
                    break;
                case Outlook.OlDisplayType.olRemoteUser:
                    Group = "Exchange（外部）";
                    Tooltip = $"[{Group}] {Address}";
                    break;
                case Outlook.OlDisplayType.olDistList:
                case Outlook.OlDisplayType.olPrivateDistList:
                    Group = "連絡先リスト";
                    Tooltip = $"[{Group}] {recp.Name}";
                    break;
                case Outlook.OlDisplayType.olAgent:
                    Group = "エージェント";
                    Tooltip = $"[{Group}] {recp.Name}";
                    break;
                case Outlook.OlDisplayType.olForum:
                    Group = "フォーラム";
                    Tooltip = $"[{Group}] {recp.Name}";
                    break;
                default:
                    Group = "不明な宛先";
                    Tooltip = $"[{Group}] {recp.Name}";
                    break;
            }
        }

        private static string GetDomain(string addr)
        {
            return addr.Substring(addr.IndexOf('@') + 1);
        }

        private static string GetSendType(Outlook.Recipient recp)
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