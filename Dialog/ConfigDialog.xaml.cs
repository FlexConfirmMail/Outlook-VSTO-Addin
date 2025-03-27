using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FlexConfirmMail.Dialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfigDialog : Window
    {
        Config _config = new Config();
        Config _default = new Config();
        Config _local = new Config();

        public ConfigDialog()
        {
            InitializeComponent();

            QueueLogger.Log("===== Open ConfigDialog() =====");

            if (Global.EnableGPO)
            {
                _default = Loader.LoadFromReg(RegistryPath.DefaultPolicy);
                _config.Merge(_default);
            }
            Config defaultWithFile = Loader.LoadFromDir(StandardPath.GetDefaultConfigDir());
            _default.Merge(defaultWithFile);
            _config.Merge(defaultWithFile);

            _local = Loader.LoadFromDir(StandardPath.GetUserDir());
            _config.Merge(_local);

            // Common
            CountEnabled.IsChecked = _config.CountEnabled;
            CountAllowSkip.IsChecked = _config.CountAllowSkip;
            CountSeconds.Text = _config.CountSeconds.ToString();
            SafeBccEnabled.IsChecked = _config.SafeBccEnabled;
            SafeBccThreshold.Text = _config.SafeBccThreshold.ToString();
            MainSkipIfNoExt.IsChecked = _config.MainSkipIfNoExt;

            // TrustedDomains
            if (_default.Modified.Contains(ConfigOption.TrustedDomains))
            {
                TrustedDomains.Text = GetTrustedDomainsPolicy();
            }
            else if (_local.Modified.Contains(ConfigOption.TrustedDomains))
            {
                TrustedDomains.Text = ReadLocalConfig("TrustedDomains.txt");
            }
            else
            {
                TrustedDomains.Text = Properties.Resources.TrustedDomainsTemplate;
            }

            // UnsafeDomains
            if (_default.Modified.Contains(ConfigOption.UnsafeDomains))
            {
                UnsafeDomains.Text = GetUnsafeDomainsPolicy();
            }
            else if (_local.Modified.Contains(ConfigOption.UnsafeDomains))
            {
                UnsafeDomains.Text = ReadLocalConfig("UnsafeDomains.txt");
            }
            else
            {
                UnsafeDomains.Text = Properties.Resources.UnsafeDomainsTemplate;
            }
            UntrustUnsafeRecipients.IsChecked = _config.UntrustUnsafeRecipients;

            // UnsafeFiles
            if (_default.Modified.Contains(ConfigOption.UnsafeFiles))
            {
                UnsafeFiles.Text = GetUnsafeFilesPolicy();
            }
            else if (_local.Modified.Contains(ConfigOption.UnsafeFiles))
            {
                UnsafeFiles.Text = ReadLocalConfig("UnsafeFiles.txt");
            }
            else
            {
                UnsafeFiles.Text = Properties.Resources.UnsafeFilesTemplate;
            }
        }

        private string GetTrustedDomainsPolicy()
        {
            string template = Properties.Resources.TrustedDomainsPolicy;
            string policy = String.Join("\n# ", _default.TrustedDomains);
            string example = _local.Modified.Contains(ConfigOption.TrustedDomains) ?
                             String.Join("\n", _local.TrustedDomains) :
                             Properties.Resources.TrustedDomainsExample;
            return String.Format(template, policy, example);
        }

        private string GetUnsafeDomainsPolicy()
        {
            string template = Properties.Resources.UnsafeDomainsPolicy;
            string policy = String.Join("\n# ", _default.UnsafeDomains);
            string example = _local.Modified.Contains(ConfigOption.UnsafeDomains) ?
                             String.Join("\n", _local.UnsafeDomains) :
                             Properties.Resources.UnsafeDomainsExample;
            return String.Format(template, policy, example);
        }

        private string GetUnsafeFilesPolicy()
        {
            string template = Properties.Resources.UnsafeFilesPolicy;
            string policy = String.Join("\n# ", _default.UnsafeFiles);
            string example = _local.Modified.Contains(ConfigOption.UnsafeFiles) ?
                             String.Join("\n", _local.UnsafeFiles) :
                             Properties.Resources.UnsafeFilesExample;
            return String.Format(template, policy, example);
        }

        private string Serialize(ConfigOption opt, bool? cur, bool? def)
        {
            if (_local.Modified.Contains(opt) || (cur != def))
            {
                return $"{opt} = {cur}\n";
            }
            return "";
        }

        private string Serialize(ConfigOption opt, string cur, string def)
        {
            if (_local.Modified.Contains(opt) || (cur != def))
            {
                return $"{opt} = {cur}\n";
            }
            return "";
        }

        private string SerializeCommon()
        {
            string text = "";
            text += Serialize(ConfigOption.CountEnabled,
                              CountEnabled.IsChecked,
                              _default.CountEnabled);
            text += Serialize(ConfigOption.CountAllowSkip,
                              CountAllowSkip.IsChecked,
                              _default.CountAllowSkip);
            text += Serialize(ConfigOption.CountSeconds,
                              CountSeconds.Text,
                              _default.CountSeconds.ToString());
            text += Serialize(ConfigOption.SafeBccEnabled,
                              SafeBccEnabled.IsChecked,
                              _default.SafeBccEnabled);
            text += Serialize(ConfigOption.SafeBccThreshold,
                              SafeBccThreshold.Text,
                              _default.SafeBccThreshold.ToString());
            text += Serialize(ConfigOption.MainSkipIfNoExt,
                              MainSkipIfNoExt.IsChecked,
                              _default.MainSkipIfNoExt);
            text += Serialize(ConfigOption.UntrustUnsafeRecipients,
                              UntrustUnsafeRecipients.IsChecked,
                              _default.UntrustUnsafeRecipients);
            return text;
        }

        private string ReadLocalConfig(string file)
        {
            string path = Path.Combine(StandardPath.GetUserDir(), file);
            try
            {
                return File.ReadAllText(path);
            }
            catch (IOException)
            {
                return "";
            }
        }

        private bool SaveFile(string basedir, string file, string content)
        {
            try
            {
                string path = System.IO.Path.Combine(basedir, file);
                string tmp = $"{path}.{GetTimestamp()}.txt";
                System.IO.Directory.CreateDirectory(basedir);
                System.IO.File.WriteAllText(tmp, content);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Replace(tmp, path, null);
                }
                else
                {
                    System.IO.File.Move(tmp, path);
                }
                QueueLogger.Log($"* Write {file} ({content.Length} bytes)");
                return true;
            }
            catch (Exception e)
            {
                QueueLogger.Log(e);
                return false;
            }
        }

        private string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssffff");
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (tabAboutAddon.IsSelected)
            {
                VersionInfo.Content = $"{Global.AppName} v{Global.Version} {Global.Edition}";
                TextLog.Text = String.Join("\n", QueueLogger.Get());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            QueueLogger.Log("* Save button clicked. closing...");
            SaveFile(StandardPath.GetUserDir(), "Common.txt", SerializeCommon());
            SaveFile(StandardPath.GetUserDir(), "TrustedDomains.txt", TrustedDomains.Text);
            SaveFile(StandardPath.GetUserDir(), "UnsafeDomains.txt", UnsafeDomains.Text);
            SaveFile(StandardPath.GetUserDir(), "UnsafeFiles.txt", UnsafeFiles.Text);
            DialogResult = true;
        }
    }
}
