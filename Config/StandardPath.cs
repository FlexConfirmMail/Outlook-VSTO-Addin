using System;
using System.IO;

namespace FlexConfirmMail
{
    public class StandardPath
    {
        public static string GetUserDir()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, Global.AppName);
        }

        public static string GetDefaultConfigDir()
        {
            string dllDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(dllDirectory, ConfigPath.DefaultConfigDirName);
        }
    }
}