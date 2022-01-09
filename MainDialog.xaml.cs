using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CheckMyMail
{
    /// <summary>
    /// MainDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MainDialog : Window
    {
        public MainDialog()
        {
            InitializeComponent();
        }

        private List<MailRecipient> GetRecipients(Outlook.MailItem mail)
        {
            var list = new List<MailRecipient>();

            foreach (Outlook.Recipient recp in mail.Recipients)
            {
                list.Add(new MailRecipient(recp));
            }

            list.Sort(CompareByAddress);
            return list;
        }

        public int CompareByAddress(MailRecipient x, MailRecipient y)
        {
            if (x.DispOrder != y.DispOrder)
            {
                return x.DispOrder - y.DispOrder;
            }
            var ret = String.Compare(x.Group, y.Group);
            if (ret != 0)
            {
                return ret;
            }
            ret = String.Compare(x.RecipientType, y.RecipientType);
            if (ret != 0)
            {
                return -ret;
            }
            return String.Compare(x.Address, y.Address);
        }

        public void LoadMail(Outlook.MailItem mail, Config config)
        {
            StackPanel sp;
            CheckBox cb;

            // Show the mail subject in title bar
            this.Title = $"{mail.Subject} - CheckMyMail";

            var groups = new HashSet<string>();
            foreach (MailRecipient recp in GetRecipients(mail))
            {
                if (config.TrustedDomains.Contains(recp.Group))
                {
                    sp = spTrusted;
                }
                else
                {
                    sp = spExt;
                }

                if (!groups.Contains(recp.Group))
                {
                    var label = new Label
                    {
                        Content = recp.Group
                    };
                    label.FontWeight = FontWeights.Bold;
                    label.Padding = new Thickness(0, 4, 0, 4);
                    sp.Children.Add(label);
                    groups.Add(recp.Group);
                }
                cb = new CheckBox
                {
                    Content = $"{recp.RecipientType,-3}: {recp.Address}",
                    ToolTip = recp.Tooltip
                };
                cb.Click += HandleClickCB;
                cb.Margin = new Thickness(7, 2, 0, 2);
                cb.MouseEnter += HandleMouseEnter;
                cb.MouseLeave += HandleMouseLeave;
                sp.Children.Add(cb);
            }

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                cb = new CheckBox { Content = item.FileName };
                cb.Click += HandleClickCB;
                cb.MouseEnter += HandleMouseEnter;
                cb.MouseLeave += HandleMouseLeave;
                spFile.Children.Add(cb);
            }
        }

        void HandleMouseEnter(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).Foreground = System.Windows.Media.Brushes.SteelBlue;
        }

        void HandleMouseLeave(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).Foreground = System.Windows.Media.Brushes.Black;
        }

        void HandleClickCB(object sender, RoutedEventArgs e)
        {
            ButtonOK.IsEnabled = IsAllChecked(spTrusted) && IsAllChecked(spExt) && IsAllChecked(spFile);
        }

        void HandleClickOK(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private static bool IsAllChecked(StackPanel sp)
        {
            foreach (UIElement e in sp.Children)
            {
                if (e is CheckBox && ((CheckBox)e).IsChecked != true)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
