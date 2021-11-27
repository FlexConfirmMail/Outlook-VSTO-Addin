using System;
using System.Collections.Generic;
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
                var d1 = r1.Address.Substring(r1.Address.IndexOf('@') + 1);
                var d2 = r1.Address.Substring(r1.Address.IndexOf('@') + 1);

                int domainOrder = String.Compare(d1, d2);
                if (domainOrder != 0)
                {
                    return domainOrder;
                }

                int typeOrder = ((int)r1.Type).CompareTo((int)r2.Type);
                if (typeOrder != 0)
                {
                    return typeOrder;
                }
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
                    new string[] { GetRecipientType(recp), address },
                    groups[domain]));
            }

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                lvFile.Items.Add(item.FileName);
            }

            chTrustedAddress.Width = -2;
            chExtAddress.Width = -2;
            chFileName.Width = -2;
        }
        private bool AllChecked(ListView lv)
        {
            return lv.CheckedItems.Count == lv.Items.Count;
        }

        private void HighlightDone(ListView lv, bool done)
        {
            if (done)
            {
                lv.BackColor = System.Drawing.Color.FromName("lavender");
            }
            else
            {
                lv.BackColor = System.Drawing.Color.FromName("Window");
            }
        }

        private void UpdateView()
        {
            var bTrusted = AllChecked(lvTrusted);
            var bExt = AllChecked(lvExt);
            var bFile = AllChecked(lvFile);

            HighlightDone(lvTrusted, bTrusted);
            HighlightDone(lvExt, bExt);
            HighlightDone(lvFile, bFile);

            if (bTrusted && bExt && bFile)
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

        private long LastCheckedTime = 0;
        private ListViewItem LastCheckedItem = null;

        private void HandleMouseDown(ListView lv, MouseEventArgs e)
        {
            var item = lv.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                item.Checked = !item.Checked; ;
            }
        }

        private void HandleItemCheck(ListView lv, ItemCheckEventArgs e)
        {
            if (e.Index > -1)
            {
                var elapsed = DateTime.Now.Ticks - LastCheckedTime;
                var item = lv.Items[e.Index];
                if (item == LastCheckedItem && elapsed < 100 * TimeSpan.TicksPerMillisecond)
                {
                    e.NewValue = e.CurrentValue;
                    return;
                }
                LastCheckedTime = DateTime.Now.Ticks;
                LastCheckedItem = item;
            }
        }

        private void HandleMouseMove(ListView lv, MouseEventArgs e)
        {
            var hover = lv.GetItemAt(e.X, e.Y);
            foreach (ListViewItem item in lv.Items)
            {
                if (item == hover)
                {
                    item.BackColor = System.Drawing.Color.LightBlue;
                }
                else
                {
                    item.BackColor = default(System.Drawing.Color);
                }
            }
        }

        private void HandleMouseLeave(ListView lv, EventArgs e)
        {
            foreach (ListViewItem item in lv.Items)
            {
                item.BackColor = default(System.Drawing.Color);
            }
        }

        private void lvTrusted_MouseDown(object sender, MouseEventArgs e)
        {
            HandleMouseDown(lvTrusted, e);
        }

        private void lvTrusted_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleItemCheck(lvTrusted, e);
        }

        private void lvTrusted_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void lvTrusted_MouseMove(object sender, MouseEventArgs e)
        {
            HandleMouseMove(lvTrusted, e);
        }

        private void lvTrusted_MouseLeave(object sender, EventArgs e)
        {
            HandleMouseLeave(lvTrusted, e);
        }

        private void lvTrusted_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void lvExt_MouseDown(object sender, MouseEventArgs e)
        {
            HandleMouseDown(lvExt, e);
        }

        private void lvExt_MouseMove(object sender, MouseEventArgs e)
        {
            HandleMouseMove(lvExt, e);
        }

        private void lvExt_MouseLeave(object sender, EventArgs e)
        {
            HandleMouseLeave(lvExt, e);
        }

        private void lvExt_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleItemCheck(lvExt, e);
        }

        private void lvExt_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void lvExt_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void lvFile_MouseDown(object sender, MouseEventArgs e)
        {
            HandleMouseDown(lvFile, e);
        }

        private void lvFile_MouseMove(object sender, MouseEventArgs e)
        {
            HandleMouseMove(lvFile, e);
        }

        private void lvFile_MouseLeave(object sender, EventArgs e)
        {
            HandleMouseLeave(lvFile, e);
        }

        private void lvFile_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            HandleItemCheck(lvFile, e);
        }

        private void lvFile_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateView();
        }

        private void lvFile_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }
    }
}
