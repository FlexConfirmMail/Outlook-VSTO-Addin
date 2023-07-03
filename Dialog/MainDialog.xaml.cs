using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace FlexConfirmMail.Dialog
{
    public partial class MainDialog : Window
    {
        private Outlook.MailItem _mail;
        private Config _config;

        public MainDialog()
        {
            InitializeComponent();
        }

        public MainDialog(Config config, Outlook.MailItem mail)
        {
            InitializeComponent();
            config.RebuildPatterns();
            _config = config;
            _mail = mail;

            QueueLogger.Log("===== Open MainDialog() =====");

            // Show the subject string in title bar
            Title = $"{mail.Subject} - {Global.AppName} v{Global.Version} {Global.Edition}";

            // Render contents in the dialog
            RenderMain();
        }

        public bool SkipConfirm()
        {
            if (_config.MainSkipIfNoExt)
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
                if (!IsEmbeddedImage(item))
                {
                    spFile.Children.Add(GetCheckBox(
                        string.Format(Properties.Resources.MainFilesWarning, item.FileName),
                        item.FileName));
                }
            }
        }

        private bool IsEmbeddedImage(Outlook.Attachment item)
        {
            // Outlook embeds images into HTML Email like this:
            //
            //   <img width=1806 height=475 src="cid:image002.png@01D87765.44471120">
            //
            return _mail.HTMLBody.Contains($"cid:{item.FileName}");
        }

        private HashSet<string> GetHashSet(List<string> list)
        {
            HashSet<string> ret = new HashSet<string>();
            HashSet<string> exclude = new HashSet<string>();
            foreach (string entry in list)
            {
                if (entry.StartsWith("-"))
                {
                    exclude.Add(entry.Substring(1));
                }
                else
                {
                    ret.Add(entry);
                }
            }
            ret.ExceptWith(exclude);
            return ret;
        }

        private List<string> ToLower(List<string> list)
        {
            List<string> ret = new List<string>();
            foreach (string entry in list)
            {
                ret.Add(entry.ToLower());
            }
            return ret;
        }

        private bool IsTrustedDomain(string domain, HashSet<string> trusted)
        {
            // Note: DOMAIN_EXCHANGE basically means "LegacyDN recipients whose
            // SMTP address we don't know". We assume they are internal users,
            // since it implies that they reside in Active Direcotry.
            if (domain == RecipientInfo.DOMAIN_EXCHANGE)
            {
                return true;
            }

            try
            {
                return Regex.IsMatch(domain, _config.TrustedDomainsPattern, RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException) { }

            return false;
        }

        private void RenderTrustedList(List<RecipientInfo> recipients)
        {
            HashSet<string> seen = new HashSet<string>();
            HashSet<string> trusted = GetHashSet(ToLower(_config.TrustedDomains));

            foreach (RecipientInfo info in recipients)
            {
                if (IsTrustedDomain(info.Domain, trusted))
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
            HashSet<string> trusted = GetHashSet(ToLower(_config.TrustedDomains));

            foreach (RecipientInfo info in list)
            {
                if (!IsTrustedDomain(info.Domain, trusted))
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
            HashSet<string> seen = new HashSet<string>();

            foreach (RecipientInfo info in recipients)
            {
                if (!seen.Contains(info.Domain))
                {
                    try
                    {
                        if (Regex.IsMatch(info.Domain, _config.UnsafeDomainsPattern, RegexOptions.IgnoreCase))
                        {
                            spFile.Children.Add(GetWarnCheckBox(
                                string.Format(Properties.Resources.MainUnsafeDomainsWarning, info.Domain),
                                Properties.Resources.MainUnsafeDomainsWarningHint
                            ));
                        }
                    }
                    catch (RegexMatchTimeoutException) { }
                    seen.Add(info.Domain);
                }
            }
        }

        private void CheckUnsafeFiles()
        {
            HashSet<string> notsafe = GetHashSet(_config.UnsafeFiles);

            foreach (Outlook.Attachment item in _mail.Attachments)
            {
                HashSet<string> seen = new HashSet<string>();
                try
                {
                    foreach (Match match in Regex.Matches(item.FileName, _config.UnsafeFilesPattern, RegexOptions.IgnoreCase))
                    {
                        string lowerValue = match.Value.ToLower();
                        if (seen.Contains(lowerValue))
                        {
                            continue;
                        }
                        spFile.Children.Add(GetWarnCheckBox(
                            string.Format(Properties.Resources.MainUnsafeFilesWarning, match.Value),
                            Properties.Resources.MainUnsafeFilesWarningHint
                        ));
                        seen.Add(lowerValue);
                    }
                }
                catch (RegexMatchTimeoutException) { }
            }
        }

        private void CheckSafeBcc(List<RecipientInfo> recipients)
        {
            if (!_config.SafeBccEnabled)
            {
                return;
            }

            int threshold = _config.SafeBccThreshold;
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

                    string.Format(Properties.Resources.MainSafeBccWarning, threshold),
                    Properties.Resources.MainSafeBccWarningHint
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
            cb.Content = title.Replace("_", "__");
            cb.ToolTip = help;
            cb.Margin = new Thickness(7, 2, 0, 2);
            cb.Click += CheckBox_Click;
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

        private void CheckAllTrusted_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in spTrusted.Children)
            {
                if (child is CheckBox)
                {
                    ((CheckBox)child).IsChecked = true;
                }
            }
            ButtonOK.IsEnabled = IsAllChecked(spTrusted) && IsAllChecked(spExt) && IsAllChecked(spFile);
        }
    }
}
