using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace FlexConfirmMail.Dialog
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
            var trusted = new List<RecipientInfo>();
            var ext = new List<RecipientInfo>();

            foreach (Outlook.Recipient recp in mail.Recipients)
            {
                var info = new RecipientInfo(recp);
                if (config.TrustedDomains.Contains(info.Domain))
                {
                    trusted.Add(info);
                }
                else
                {
                    ext.Add(info);
                }
            }
            RenderAddressList(spTrusted, trusted);
            RenderAddressList(spExt, ext);

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                spFile.Children.Add(NewCheckBox(item.FileName, item.FileName));
            }

            var all = trusted.Concat(ext).ToList();
            CheckDomainCount(all);
            CheckUnsafeDomain(all, config);

            /* Show the subject string in title bar */
            Title = $"{mail.Subject} - FlexConfirmMail";
        }

        private void CheckUnsafeDomain(List<RecipientInfo> list, Config config)
        {
            var domains = new HashSet<string>();
            foreach (RecipientInfo info in list)
            {
                if (config.UnsafeDomains.Contains(info.Domain) && !domains.Contains(info.Domain))
                {
                    spFile.Children.Add(NewCheckBox(
                        $"[警告] 注意が必要なドメイン（{info.Domain}）が宛先に含まれています。",
                        "このドメインは誤送信の可能性が高いため、再確認を促す警告を出してします。"
                    ));
                    domains.Add(info.Domain);
                }
            }
        }

        private void CheckDomainCount(List<RecipientInfo> list)
        {
            var domains = new HashSet<string>();
            foreach (RecipientInfo info in list)
            {
                if (info.IsSMTP && info.Type != "Bcc" && !domains.Contains(info.Domain))
                {
                    domains.Add(info.Domain);
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

        private void RenderAddressList(StackPanel sp, List<RecipientInfo> list)
        {
            var domains = new HashSet<string>();
            list.Sort();

            foreach (RecipientInfo info in list)
            {
                if (!domains.Contains(info.Domain))
                {
                    sp.Children.Add(NewDomainLabel(info.Domain));
                    domains.Add(info.Domain);
                }
                sp.Children.Add(NewCheckBox($"{info.Type,-3}: {info.Address}", info.Help));
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

        private void HandleMouseEnter(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).Foreground = System.Windows.Media.Brushes.SteelBlue;
        }

        private void HandleMouseLeave(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).Foreground = System.Windows.Media.Brushes.Black;
        }

        private void HandleClickCB(object sender, RoutedEventArgs e)
        {
            ButtonOK.IsEnabled = IsAllChecked(spTrusted) && IsAllChecked(spExt) && IsAllChecked(spFile);
        }

        private void HandleClickOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
