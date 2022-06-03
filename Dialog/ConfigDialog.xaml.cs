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
        Config _config;

        public ConfigDialog()
        {
            InitializeComponent();

            QueueLogger.Log("Open ConfigDialog()");

            _config = Loader.LoadFromDir(StandardPath.GetUserDir());

            // Common
            CountEnabled.IsChecked = _config.CountEnabled;
            CountAllowSkip.IsChecked = _config.CountAllowSkip;
            CountSeconds.Text = _config.CountSeconds.ToString();
            SafeBccEnabled.IsChecked = _config.SafeBccEnabled;
            SafeBccThreshold.Text = _config.SafeBccThreshold.ToString();
            MainSkipIfNoExt.IsChecked = _config.MainSkipIfNoExt;

            // TrustedDomains
            string text;
            text = ReadFile(Path.Combine(StandardPath.GetUserDir(), "TrustedDomains.txt"));
            if (text != null)
            {
                TrustedDomains.Text = text;
            }
            else
            {
                TrustedDomains.Text = Properties.Resources.ConfigTrustedDomainsTemplate;
            }

            // UnsafeDomains
            text = ReadFile(Path.Combine(StandardPath.GetUserDir(), "UnsafeDomains.txt"));
            if (text != null)
            {
                UnsafeDomains.Text = text;
            }
            else
            {
                UnsafeDomains.Text = Properties.Resources.ConfigUnsafeDomainsTemplate;
            }

            // UnsafeFiles
            text = ReadFile(Path.Combine(StandardPath.GetUserDir(), "UnsafeFiles.txt"));
            if (text != null)
            {
                UnsafeFiles.Text = text;
            }
            else
            {
                UnsafeFiles.Text = Properties.Resources.ConfigUnsafeFilesTemplate;
            }
        }

        private string SerializeCommon()
        {
            return $@"
{ConfigOption.CountEnabled} = {CountEnabled.IsChecked.ToString()}
{ConfigOption.CountAllowSkip} = {CountAllowSkip.IsChecked.ToString()}
{ConfigOption.CountSeconds} = {CountSeconds.Text}
{ConfigOption.SafeBccEnabled} = {SafeBccEnabled.IsChecked.ToString()}
{ConfigOption.SafeBccThreshold} = {SafeBccThreshold.Text}
{ConfigOption.MainSkipIfNoExt} = {MainSkipIfNoExt.IsChecked.ToString()}";
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
                VersionInfo.Content = $"{Global.AppName} v{Global.Version}";
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

        private string ReadFile(string path)
        {
             try
             {
                 return File.ReadAllText(path);
             }
             catch (IOException)
             {
                 return null;
             }
        }
    }
}
