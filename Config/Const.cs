using System.Collections.Generic;

namespace FlexConfirmMail.Config
{
    public class Global
    {
        public static readonly string Version = "22.0-rc2";
        public static readonly string AppName = "FlexConfirmMail";
    }

    public class ConfigFile
    {
        public static readonly string Common = "Common.txt";
        public static readonly string TrustedDomains = "TrustedDomains.txt";
        public static readonly string UnsafeDomains = "UnsafeDomains.txt";
        public static readonly string UnsafeFiles = "UnsafeFiles.txt";
    }

    public class ConfigOption
    {
        public static string CountEnabled = "CountEnabled";
        public static string CountSeconds = "CountSeconds";
        public static string CountAllowSkip = "CountAllowSkip";
        public static string SafeBccEnabled = "SafeBccEnabled";
        public static string SafeBccThreshold = "SafeBccThreshold";
    }

    public class ConfigDefault
    {
        static public Dictionary<string, string> Get()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add(ConfigOption.CountEnabled, "true");
            options.Add(ConfigOption.CountSeconds, "3");
            options.Add(ConfigOption.CountAllowSkip, "true");
            options.Add(ConfigOption.SafeBccEnabled, "true");
            options.Add(ConfigOption.SafeBccThreshold, "4");
            return options;
        }
    }
}