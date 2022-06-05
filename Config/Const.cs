using System.Collections.Generic;

namespace FlexConfirmMail
{
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