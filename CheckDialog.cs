using System;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CheckMyMail
{
    public partial class CheckDialog : Form
    {
        public CheckDialog()
        {
            InitializeComponent();
        }
        public void LoadMail(Outlook.MailItem mail)
        {
            foreach (Outlook.Recipient item in mail.Recipients)
            {
                clbExt.Items.Add(DispAddress(item));
            }
        }
        private string DispAddress(Outlook.Recipient item)
        {
            switch (item.Type)
            {
                case (int)Outlook.OlMailRecipientType.olBCC:
                    return " Bcc:    " + item.Address;
                case (int)Outlook.OlMailRecipientType.olCC:
                    return " Cc :    " + item.Address;
                default:
                    return " To :    " + item.Address;
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (AllChecked(clbExt))
            {
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }

        }

        private bool AllChecked(CheckedListBox clb)
        {
            return clb.CheckedItems.Count == clb.Items.Count;
        }
    }
}
