using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Outlook = Microsoft.Office.Interop.Outlook;
using FlexConfirmMail.Config;

namespace FlexConfirmMail.Dialog
{
    /// <summary>
    /// MainDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MainDialog : Window
    {
        private ConfigData _config;

        public MainDialog()
        {
            InitializeComponent();
        }

        public MainDialog(ConfigData config)
        {
            InitializeComponent();
            _config = config;
        }

        public void LoadMail(Outlook.MailItem mail)
        {
            var trusted = new List<RecipientInfo>();
            var ext = new List<RecipientInfo>();

            foreach (Outlook.Recipient recp in mail.Recipients)
            {
                HashSet<string> hsTrusted = _config.GetHashSet(ConfigFile.TrustedDomains);
                RecipientInfo info = new RecipientInfo(recp);
                if (hsTrusted.Contains(info.Domain) || info.Domain == RecipientInfo.DOMAIN_EXCHANGE)
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
                spFile.Children.Add(NewCheckBox($"[添付ファイル] {item.FileName}", item.FileName));
            }

            var all = trusted.Concat(ext).ToList();
            CheckDomainCount(all);
            CheckUnsafeDomain(all);

            /* Show the subject string in title bar */
            Title = $"{mail.Subject} - FlexConfirmMail";
        }

        private void CheckUnsafeDomain(List<RecipientInfo> list)
        {
            HashSet<string> hsUnsafe = _config.GetHashSet(ConfigFile.UnsafeDomains);
            HashSet<string> done = new HashSet<string>();

            foreach (RecipientInfo info in list)
            {
                if (done.Contains(info.Domain))
                {
                    continue;
                }

                if (hsUnsafe.Contains(info.Domain))
                {
                    spFile.Children.Add(NewCheckBox(
                        $"[警告] 注意が必要なドメイン（{info.Domain}）が宛先に含まれています。",
                        "このドメインは誤送信の可能性が高いため、再確認を促す警告を出してします。"
                    ));
                }
                done.Add(info.Domain);
            }
        }

        private void CheckDomainCount(List<RecipientInfo> list)
        {
            if (!_config.GetBool(ConfigOption.SafeBccEnabled))
            {
                return;
            }

            int threshold = _config.GetInt(ConfigOption.SafeBccThreshold);
            if (threshold < 1)
            {
                return;
            }

            var domains = new HashSet<string>();
            foreach (RecipientInfo info in list)
            {
                if (info.IsSMTP && info.Type != "Bcc" && !domains.Contains(info.Domain))
                {
                    domains.Add(info.Domain);
                }
            }
            if (domains.Count >= threshold)
            {
                spFile.Children.Add(NewCheckBox(
                    $"[警告] To・Ccに{threshold}件以上のドメインが含まれています。",
                    @"宛先に多数のドメインが検知されました。
ToおよびCcに含まれるメールアドレスはすべての受取人が確認できるため、
アナウンスなどを一斉送信する場合はBccを利用して宛先リストを隠します。"
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
