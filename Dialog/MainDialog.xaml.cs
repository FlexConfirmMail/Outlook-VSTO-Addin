using FlexConfirmMail.Config;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace FlexConfirmMail.Dialog
{
    public partial class MainDialog : Window
    {
        private Outlook.MailItem _mail;
        private ConfigData _config;

        public MainDialog()
        {
            InitializeComponent();
        }

        public MainDialog(ConfigData config, Outlook.MailItem mail)
        {
            InitializeComponent();
            _config = config;
            _mail = mail;

            QueueLogger.Log("Open MainDialog()");

            // Show the subject string in title bar
            Title = $"{mail.Subject} - FlexConfirmMail";

            // Render contents in the dialog
            RenderMain();
        }

        public bool SkipConfirm()
        {
            if (_config.GetBool(ConfigOption.MainSkipIfNoExt))
            {
                if (spExt.Children.Count == 0)
                {
                    QueueLogger.Log("* No external address found. skipping...");
                    return true;
                }
            }
            return false;
        }

        private void RenderMain()
        {
            List<RecipientInfo> recipients = new List<RecipientInfo>();

            foreach (Outlook.Recipient recp in _mail.Recipients)
            {
                recipients.Add(new RecipientInfo(recp));
            }

            // Address CheckBox
            recipients.Sort();
            RenderExternalList(recipients);
            RenderTrustedList(recipients);

            // Attachments/Alerts
            CheckSafeBcc(recipients);
            CheckUnsafeDomains(recipients);
            CheckUnsafeFiles();

            foreach (Outlook.Attachment item in _mail.Attachments)
            {
                spFile.Children.Add(GetCheckBox($"[添付ファイル] {item.FileName}", item.FileName));
            }
        }

        private void RenderTrustedList(List<RecipientInfo> recipients)
        {
            HashSet<string> seen = new HashSet<string>();
            HashSet<string> trustedDomains = _config.GetHashSet(ConfigFile.TrustedDomains);

            // Assume Exchange as internal domain.
            trustedDomains.Add(RecipientInfo.DOMAIN_EXCHANGE);

            foreach (RecipientInfo info in recipients)
            {
                if (trustedDomains.Contains(info.Domain))
                {
                    if (!seen.Contains(info.Domain))
                    {
                        spTrusted.Children.Add(GetDomainLabel(info.Domain));
                        seen.Add(info.Domain);
                    }
                    spTrusted.Children.Add(GetCheckBox($"{info.Type,-3}: {info.Address}", info.Help));
                }
            }
        }

        private void RenderExternalList(List<RecipientInfo> list)
        {
            HashSet<string> seen = new HashSet<string>();
            HashSet<string> trustedDomains = _config.GetHashSet(ConfigFile.TrustedDomains);

            // Consider Exchange as internal domain.
            trustedDomains.Add(RecipientInfo.DOMAIN_EXCHANGE);

            foreach (RecipientInfo info in list)
            {
                if (!trustedDomains.Contains(info.Domain))
                {
                    if (!seen.Contains(info.Domain))
                    {
                        spExt.Children.Add(GetDomainLabel(info.Domain));
                        seen.Add(info.Domain);
                    }
                    spExt.Children.Add(GetWarnCheckBox($"{info.Type,-3}: {info.Address}", info.Help));
                }
            }
        }

        private void CheckUnsafeDomains(List<RecipientInfo> recipients)
        {
            HashSet<string> unsafeDomains = _config.GetHashSet(ConfigFile.UnsafeDomains);
            HashSet<string> seen = new HashSet<string>();

            foreach (RecipientInfo info in recipients)
            {
                if (!seen.Contains(info.Domain))
                {
                    if (unsafeDomains.Contains(info.Domain))
                    {
                        spFile.Children.Add(GetWarnCheckBox(
                            $"[警告] 注意が必要なドメイン（{info.Domain}）が宛先に含まれています。",
                            "このドメインは誤送信の可能性が高いため、再確認を促す警告を出してします。"
                        ));
                    }
                    seen.Add(info.Domain);
                }
            }
        }

        private void CheckUnsafeFiles()
        {
            HashSet<string> unsafeFiles = _config.GetHashSet(ConfigFile.UnsafeFiles);

            foreach (Outlook.Attachment item in _mail.Attachments)
            {
                foreach (string keyword in unsafeFiles)
                {
                    if (item.FileName.Contains(keyword))
                    {
                        spFile.Children.Add(GetWarnCheckBox(
                            $"[警告] 注意が必要なファイル名（{keyword}）が含まれています。",
                            $"添付ファイル「{item.FileName}」に注意が必要な単語が含まれているため、" +
                            $"再確認を促す警告を出しています。"
                        ));
                        break;
                    }
                }
            }
        }

        private void CheckSafeBcc(List<RecipientInfo> recipients)
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

            HashSet<string> domains = new HashSet<string>();
            foreach (RecipientInfo info in recipients)
            {
                if (info.IsSMTP && info.Type != "Bcc" && !domains.Contains(info.Domain))
                {
                    domains.Add(info.Domain);
                }
            }

            if (domains.Count >= threshold)
            {
                spFile.Children.Add(GetWarnCheckBox(
                    $"[警告] To・Ccに{threshold}件以上のドメインが含まれています。",
                    $"宛先に多数のドメインが検知されました。" +
                    $"ToおよびCcに含まれるメールアドレスはすべての受取人が確認できるため、" +
                    $"アナウンスなどを一斉送信する場合はBccを利用して宛先リストを隠します。"
                ));
            }
        }

        private Label GetDomainLabel(string title)
        {
            Label label = new Label();
            label.Content = title;
            label.FontWeight = FontWeights.Bold;
            label.Padding = new Thickness(0, 4, 0, 4);
            return label;
        }

        private CheckBox GetCheckBox(string title, string help)
        {
            CheckBox cb = new CheckBox();
            cb.Content = title;
            cb.ToolTip = help;
            cb.Margin = new Thickness(7, 2, 0, 2);
            cb.Click +=  CheckBox_Click;
            cb.MouseEnter += CheckBox_MouseEnter;
            cb.MouseLeave += CheckBox_MouseLeave;
            return cb;
        }

        private CheckBox GetWarnCheckBox(string title, string help)
        {
            CheckBox cb = GetCheckBox(title, help);
            cb.Foreground = System.Windows.Media.Brushes.Firebrick;
            cb.FontWeight = FontWeight.FromOpenTypeWeight(500);
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

        private void CheckBox_MouseEnter(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Foreground == System.Windows.Media.Brushes.Firebrick)
            {
                cb.Foreground = System.Windows.Media.Brushes.RosyBrown;
            }
            else
            {
                cb.Foreground = System.Windows.Media.Brushes.SteelBlue;
            }
        }

        private void CheckBox_MouseLeave(object sender, RoutedEventArgs e)
        {

            CheckBox cb = (CheckBox)sender;
            if (cb.Foreground == System.Windows.Media.Brushes.RosyBrown)
            {
                cb.Foreground = System.Windows.Media.Brushes.Firebrick;
            }
            else
            {
                cb.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ButtonOK.IsEnabled = IsAllChecked(spTrusted) && IsAllChecked(spExt) && IsAllChecked(spFile);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            QueueLogger.Log("* Send button clicked. closing...");
            DialogResult = true;
        }
    }
}
