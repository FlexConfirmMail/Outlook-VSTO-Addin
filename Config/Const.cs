using System.Collections.Generic;

namespace FlexConfirmMail
{
    public class Global
    {
        public static readonly string Version = "22.1-rc2";
        public static readonly string AppName = "FlexConfirmMail";
    }

    public enum ConfigOption
    {
        CountEnabled,
        CountSeconds,
        CountAllowSkip,
        SafeBccEnabled,
        SafeBccThreshold,
        MainSkipIfNoExt,
        TrustedDomains,
        UnsafeDomains,
        UnsafeFiles
    }
}