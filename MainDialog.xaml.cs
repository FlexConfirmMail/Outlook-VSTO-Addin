using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
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

        public void LoadMail(Outlook.MailItem mail, Config config)
        {
            var trusted = new List<MailRecipient>();
            var ext = new List<MailRecipient>();

            foreach (Outlook.Recipient recp in mail.Recipients)
            {
                var mr = MailRecipientFactory.Create(recp);
                if (config.TrustedDomains.Contains(mr.Domain))
                {
                    trusted.Add(mr);
                }
                else
                {
                    ext.Add(mr);
                }
            }
            RenderAddressList(spTrusted, trusted);
            RenderAddressList(spExt, ext);

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                spFile.Children.Add(NewCheckBox(item.FileName, item.FileName));
            }
            CheckDomainCount(trusted.Concat(ext).ToList());

            /* Show the subject string in title bar */
            this.Title = $"{mail.Subject} - CheckMyMail";
        }

        private void CheckDomainCount(List<MailRecipient> list)
        {
            var domains = new HashSet<string>();
            foreach (MailRecipient recp in list)
            {
                if (recp.IsSMTP && recp.Type != "Bcc" && !domains.Contains(recp.Domain))
                {
                    domains.Add(recp.Domain);
                }
            }
            if (domains.Count > 3)
            {
                spFile.Children.Add(NewCheckBox(
                    "[警告] To・Ccに4件以上のドメインが含まれています。",
                    @"多数のドメインが検知された場合の警告です。
ToおよびCcに含まれるメールアドレスはすべての受取人が確認できるため、アナウンスなどを一斉送信する場合はBccを利用して宛先リストを隠します。"
                ));
            }
        }

        private void RenderAddressList(StackPanel sp, List<MailRecipient> list)
        {
            var domains = new HashSet<string>();
            list.Sort();

            foreach (MailRecipient recp in list)
            {
                if (!domains.Contains(recp.Domain))
                {
                    sp.Children.Add(NewDomainLabel(recp.Domain));
                    domains.Add(recp.Domain);
                }
                sp.Children.Add(NewCheckBox($"{recp.Type,-3}: {recp.Address}", recp.Help));
            }
        }

        private Label NewDomainLabel(string title)
        {
            var label = new Label();
            label.Content = title;
            label.FontWeight = FontWeights.Bold;
            label.Padding = new Thickness(0, 4, 0, 4);
            return label;
        }

        private CheckBox NewCheckBox(string title, string help)
        {
            var cb = new CheckBox();
            cb.Content = title;
            cb.ToolTip = help;
            cb.Margin = new Thickness(7, 2, 0, 2);
            cb.Click += HandleClickCB;
            cb.MouseEnter += HandleMouseEnter;
            cb.MouseLeave += HandleMouseLeave;
            return cb;
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
    }
}
