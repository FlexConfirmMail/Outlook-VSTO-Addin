using System.Collections.Generic;

namespace FlexConfirmMail
{
    public class RegistryPath
    {
        public static readonly string DefaultPolicy = @"SOFTWARE\Policies\FlexConfirmMail\Default";
        public static readonly string Policy = @"SOFTWARE\Policies\FlexConfirmMail";
    }

    public class ConfigPath
    {
        public static readonly string DefaultConfigDirName = @"DefaultConfig";
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
        UnsafeFiles,
        SafeNewDomainsEnabled,
    }
}