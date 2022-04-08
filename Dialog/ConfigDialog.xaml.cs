using FlexConfirmMail.Config;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FlexConfirmMail.Dialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfigDialog : Window
    {
        public ConfigDialog()
        {
            InitializeComponent();

            QueueLogger.Log("Open ConfigDialog()");

            ConfigData config = new ConfigData();
            FileLoader loader = new FileLoader(config);
            loader.TryOptionFile(StandardPath.GetUserDir(), ConfigFile.Common);

            // Common
            CountEnabled.IsChecked = config.GetBool(ConfigOption.CountEnabled);
            CountAllowSkip.IsChecked = config.GetBool(ConfigOption.CountAllowSkip);
            CountSeconds.Text = config.GetInt(ConfigOption.CountSeconds).ToString();
            SafeBccEnabled.IsChecked = config.GetBool(ConfigOption.SafeBccEnabled);
            SafeBccThreshold.Text = config.GetInt(ConfigOption.SafeBccThreshold).ToString();
            MainSkipIfNoExt.IsChecked = config.GetBool(ConfigOption.MainSkipIfNoExt);

            // TrustedDomains
            string text;
            if (loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains, out text))
            {
                TrustedDomains.Text = text;
            }
            else
            {
                TrustedDomains.Text = Properties.Resources.ConfigTrustedDomainsTemplate;
            }

            // UnsafeDomains
            if (loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains, out text))
            {
                UnsafeDomains.Text = text;
            }
            else
            {
                UnsafeDomains.Text = Properties.Resources.ConfigUnsafeDomainsTemplate;
            }

            // UnsafeFiles
            if (loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles, out text))
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
            SaveFile(StandardPath.GetUserDir(), ConfigFile.Common, SerializeCommon());
            SaveFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains, TrustedDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains, UnsafeDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles, UnsafeFiles.Text);
            DialogResult = true;
        }
    }
}
