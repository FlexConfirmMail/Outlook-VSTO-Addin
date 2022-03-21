using System;

using System.IO;

namespace FlexConfirmMail.Config
{
    public class StandardPath
    {
        public static string GetUserDir()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, Global.AppName);
        }
    }
}