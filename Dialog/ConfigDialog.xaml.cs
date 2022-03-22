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
        private string _templateTrutedDomains = @"
# 社内の宛先として扱うドメイン ###
#
# ドメインとはメールアドレスのうち「@」の後の部分を指し、
# 以下の例のように一行に一件ずつ記載します。
#
# また、冒頭が「#」から始まる行は無視されますので、
# 更新管理にご利用下さい。
##################################

example.com
example.org";
        private string _templateUnsafeDomains = @"
# 送信時に警告するドメイン ###
#
# ドメインとはメールアドレスのうち「@」の後の部分を指し、
# 以下の例のように一行に一件ずつ記載します。
#
# また、冒頭が「#」から始まる行は無視されますので、
# 更新管理にご利用下さい。
##################################

example.com
example.org";
        private string _templateUnsafeFiles = @"
# 警告対象となるファイル名 ###
#
# 添付ファイルに含まれている場合に警告対象とする単語を
# 以下の例のように一行に一件ずつ記載します。
#
# また、冒頭が「#」から始まる行は無視されますので、
# 更新管理にご利用下さい。
##################################

社外秘
機密";

        public ConfigDialog()
        {
            InitializeComponent();

            QueueLogger.Log("Open ConfigDialog");

            ConfigData config = new ConfigData();
            FileLoader loader = new FileLoader(config);
            loader.TryOptionFile(StandardPath.GetUserDir(), ConfigFile.Common);

            // Common
            CountEnabled.IsChecked = config.GetBool(ConfigOption.CountEnabled);
            CountAllowSkip.IsChecked = config.GetBool(ConfigOption.CountAllowSkip);
            CountSeconds.Text = config.GetInt(ConfigOption.CountSeconds).ToString();
            SafeBccEnabled.IsChecked = config.GetBool(ConfigOption.SafeBccEnabled);
            SafeBccThreshold.Text = config.GetInt(ConfigOption.SafeBccThreshold).ToString();

            // TrustedDomains
            TrustedDomains.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains);
            if (String.IsNullOrWhiteSpace(TrustedDomains.Text))
            {
                TrustedDomains.Text = _templateTrutedDomains.Trim();
            }

            // UnsafeDomains
            UnsafeDomains.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains);
            if (String.IsNullOrWhiteSpace(UnsafeDomains.Text))
            {
                UnsafeDomains.Text = _templateUnsafeDomains.Trim();
            }

            // UnsafeFiles
            UnsafeFiles.Text = loader.TryRawFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles);
            if (String.IsNullOrWhiteSpace(UnsafeFiles.Text))
            {
                UnsafeFiles.Text = _templateUnsafeFiles.Trim();
            }
        }

        private string SerializeCommon()
        {
            return $@"
{ConfigOption.CountEnabled} = {CountEnabled.IsChecked.ToString()}
{ConfigOption.CountAllowSkip} = {CountAllowSkip.IsChecked.ToString()}
{ConfigOption.CountSeconds} = {CountSeconds.Text}
{ConfigOption.SafeBccEnabled} = {SafeBccEnabled.IsChecked.ToString()}
{ConfigOption.SafeBccThreshold} = {SafeBccThreshold.Text}";
        }

        private bool SaveFile(string basedir, string file, string content)
        {
            try {
                string path = System.IO.Path.Combine(basedir, file);
                string tmp = $"{path}.{GetTimestamp()}.txt";
                System.IO.File.WriteAllText(tmp, content);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Replace(tmp, path, null);
                }
                else
                {
                    System.IO.File.Move(tmp, path);
                }
                QueueLogger.Log($"- Saved {path} ({content.Length} bytes)");
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
            QueueLogger.Log("Save configurations");
            SaveFile(StandardPath.GetUserDir(), ConfigFile.Common, SerializeCommon());
            SaveFile(StandardPath.GetUserDir(), ConfigFile.TrustedDomains, TrustedDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeDomains, UnsafeDomains.Text);
            SaveFile(StandardPath.GetUserDir(), ConfigFile.UnsafeFiles, UnsafeFiles.Text);
            DialogResult = true;
        }
    }
}
