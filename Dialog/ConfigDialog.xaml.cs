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
            Configure();
        }
        private void Configure()
        {
            ConfigData config = new ConfigData();
            FileLoader loader = new FileLoader(config);
            loader.TryOptionFile(StandardPath.GetUserDir(), ConfigFile.Common);

            CountEnabled.IsChecked = config.GetBool(ConfigOption.CountEnabled);

            CountAllowSkip.IsChecked = config.GetBool(ConfigOption.CountAllowSkip);

            CountSeconds.Text = config.GetInt(ConfigOption.CountSeconds).ToString();

            SafeBccEnabled.IsChecked = config.GetBool(ConfigOption.SafeBccEnabled);

            SafeBccThreshold.Text = config.GetInt(ConfigOption.SafeBccThreshold).ToString();

            TrustedDomains.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains);

            UnsafeDomains.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains);

            UnsafeFiles.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles);
        }

        private string SerializeCommon()
        {
            return $@"
{ConfigOption.CountEnabled} = {CountEnabled.IsChecked.ToString()}
{ConfigOption.CountAllowSkip} = {CountAllowSkip.IsChecked.ToString()}
{ConfigOption.CountSeconds} = {CountSeconds.Text}
{ConfigOption.SafeBccEnabled} = {SafeBccEnabled.IsChecked.ToString()}
{ConfigOption.SafeBccThreshold} = {SafeBccThreshold.Text}
";
        }

        private bool SaveFile(string basedir, string file, string content)
        {
            try
            {
                QueueLogger.Log($"- Save {file}\n{content}");
                string path = System.IO.Path.Combine(basedir, file);
                string tmp = $"{path}.{GetTimestamp()}.txt";
                System.IO.File.WriteAllText(tmp, content);
                System.IO.File.Replace(tmp, path, null);
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
                textLog.Text = String.Join("\n", QueueLogger.Get());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            QueueLogger.Log("Save configurations");
            SaveFile(StandardPath.GetUserDir(), ConfigFile.Common, SerializeCommon());
            SaveFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains, TrustedDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains, UnsafeDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles, UnsafeFiles.Text);
            DialogResult = true;
        }

    }
}
