using System;
using System.Collections.Generic;
using System.Collections;
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

        private string GetRecipientType(Outlook.Recipient rp)
        {
            switch (rp.Type)
            {
                case (int)Outlook.OlMailRecipientType.olBCC:
                    return "Bcc";
                case (int)Outlook.OlMailRecipientType.olCC:
                    return "Cc";
                default:
                    return "To";
            }
        }

        private List<Outlook.Recipient> GetRecipients(Outlook.MailItem mail)
        {
            var list = new List<Outlook.Recipient>();

            foreach (Outlook.Recipient recp in mail.Recipients)
            {
                list.Add(recp);
            }

            list.Sort((Outlook.Recipient r1, Outlook.Recipient r2) =>
            {
                return String.Compare(r1.Address, r2.Address);
            });
            return list;
        }

        public void LoadMail(Outlook.MailItem mail, Config config)
        {
            string address;
            string domain;
            ListView listview;
            var groups = new Dictionary<string, ListViewGroup>();

            foreach (Outlook.Recipient recp in GetRecipients(mail))
            {
                int idx = recp.Address.IndexOf('@');
                if (idx < 0)
                {
                    address = recp.Name;
                    domain = Config.DOMAIN_EXCHANGE;
                }
                else
                {
                    address = recp.Address;
                    domain = recp.Address.Substring(idx + 1);
                }

                if (config.TrustedDomains.Contains(domain))
                {
                    listview = lvTrusted;
                }
                else
                {
                    listview = lvExt;
                }

                if (!groups.ContainsKey(domain))
                {
                    var group = new ListViewGroup(domain);
                    groups.Add(domain, group);
                    listview.Groups.Add(group);
                }

                listview.Items.Add(new ListViewItem(
                    new string[] {GetRecipientType(recp), address},
                    groups[domain]));
            }

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                lvFile.Items.Add(item.FileName);
            }
        }

        private long LastCheckedTime = 0;
        private ListViewItem LastCheckedItem = null;

        private void OnMouseDown(ListView lv, MouseEventArgs e)
        {
            var item = lv.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                item.Checked = !item.Checked;;
            }
        }

        private void OnItemCheck(ListView lv, ItemCheckEventArgs e)
        {
            var item = lv.Items[e.Index];
            if (item == LastCheckedItem)
            {
                var elapsed = DateTime.Now.Ticks - LastCheckedTime;
                if (elapsed < 100 * TimeSpan.TicksPerMillisecond)
                {
                    e.NewValue = e.CurrentValue;
                    return;
                }
            }
            LastCheckedTime = DateTime.Now.Ticks;
            LastCheckedItem = lv.Items[e.Index];
        }

        private void lvTrusted_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(lvTrusted, e);
        }

        private void lvTrusted_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            OnItemCheck(lvTrusted, e);
        }
        private void lvTrusted_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void lvExt_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(lvExt, e);
        }

        private void lvExt_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            OnItemCheck(lvExt, e);
        }

        private void lvExt_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void lvFile_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(lvFile, e);
        }

        private void lvFile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            OnItemCheck(lvFile, e);
        }

        private void lvFile_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (AllChecked(lvExt) && AllChecked(lvTrusted) && AllChecked(lvFile))
            {
                btnOK.Enabled = true;
                btnOK.ForeColor = System.Drawing.Color.White;
                btnOK.BackColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                btnOK.Enabled = false;
                btnOK.ForeColor = System.Drawing.Color.FromName("ControlText");
                btnOK.BackColor = System.Drawing.Color.FromName("Control");
            }
        }

        private bool AllChecked(ListView lv)
        {
            return lv.CheckedItems.Count == lv.Items.Count;
        }
    }
}
