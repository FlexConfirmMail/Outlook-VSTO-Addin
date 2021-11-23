﻿using System;
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

        private long LastSelectTime = 0;
        private ListViewItem LastSelectItem = null;

        private void OnSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                var now = DateTime.Now.Ticks;
                if (e.Item !=LastSelectItem || (now - LastSelectTime) > 300 * TimeSpan.TicksPerMillisecond)
                {
                    e.Item.Checked = !e.Item.Checked;
                }
                LastSelectTime = now;
                LastSelectItem = e.Item;
                UpdateView();
            }
        }

        private void OnItemChecked(object sender, ItemCheckedEventArgs e)
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
