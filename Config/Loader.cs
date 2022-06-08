using System;
using System.IO;
using System.Collections.Generic;

namespace FlexConfirmMail
{
    public class Loader
    {
        public static Config LoadFromDir(string basedir)
        {
            Config config = new Config();
            Dictionary<string, string> dict;
            List<string> list;

            dict = FileConfig.ReadDict(Path.Combine(basedir, "Common.txt"));
            if (dict != null)
            {
                foreach (KeyValuePair<string, string> kv in dict)
                {
                    if (Assign(config, kv.Key, kv.Value))
                    {
                        QueueLogger.Log($"* Option: {kv.Key} = {kv.Value}");
                    }
                    else
                    {
                        QueueLogger.Log($"* Unknown option: {kv.Key} = {kv.Value}");
                    }
                }
            }

            list = FileConfig.ReadList(Path.Combine(basedir, "TrustedDomains.txt"));
            if (list != null)
            {
                QueueLogger.Log("* List: " + String.Join(" ", list));
                config.TrustedDomains.AddRange(list);
                config.Modified.Add(ConfigOption.TrustedDomains);
            }

            list = FileConfig.ReadList(Path.Combine(basedir, "UnsafeDomains.txt"));
            if (list != null)
            {
                QueueLogger.Log("* List: " + String.Join(" ", list));
                config.UnsafeDomains.AddRange(list);
                config.Modified.Add(ConfigOption.UnsafeDomains);
            }

            list = FileConfig.ReadList(Path.Combine(basedir, "UnsafeFiles.txt"));
            if (list != null)
            {
                QueueLogger.Log("* List: " + String.Join(" ", list));
                config.UnsafeFiles.AddRange(list);
                config.Modified.Add(ConfigOption.UnsafeFiles);
            }
            return config;
        }

        public static Config LoadFromReg(string basedir)
        {
            Config config = new Config();
            Dictionary<string, string> dict;


            dict = RegistryConfig.ReadDict(basedir);
            if (dict != null)
            {
                foreach (KeyValuePair<string, string> kv in dict)
                {
                    if (Assign(config, kv.Key, kv.Value))
                    {
                        QueueLogger.Log($"* Option: {kv.Key} = {kv.Value}");
                    }
                    else
                    {
                        QueueLogger.Log($"* Unknown option: {kv.Key} = {kv.Value}");
                    }
                }
            }

            return config;
        }

        private static bool Assign(Config config, string key, string val)
        {
            int i;
            bool b;
            List<string> list;

            if (key == "CountEnabled")
            {
                if (ParseBool(val, out b))
                {
                    config.CountEnabled = b;
                    config.Modified.Add(ConfigOption.CountEnabled);
                    return true;
                }
            }

            if (key == "CountSeconds")
            {
                if (ParseInt(val, out i))
                {
                    config.CountSeconds = i;
                    config.Modified.Add(ConfigOption.CountSeconds);
                    return true;
                }
            }

            if (key == "CountAllowSkip")
            {
                if (ParseBool(val, out b))
                {
                    config.CountAllowSkip = b;
                    config.Modified.Add(ConfigOption.CountAllowSkip);
                    return true;
                }
            }

            if (key == "SafeBccEnabled")
            {
                if (ParseBool(val, out b))
                {
                    config.SafeBccEnabled = b;
                    config.Modified.Add(ConfigOption.SafeBccEnabled);
                    return true;
                }
            }

            if (key == "SafeBccThreshold")
            {
                if (ParseInt(val, out i))
                {
                    config.SafeBccThreshold = i;
                    config.Modified.Add(ConfigOption.SafeBccThreshold);
                    return true;
                }
            }

            if (key == "MainSkipIfNoExt")
            {
                if (ParseBool(val, out b))
                {
                    config.MainSkipIfNoExt = b;
                    config.Modified.Add(ConfigOption.MainSkipIfNoExt);
                    return true;
                }
            }

            if (key == "TrustedDomains")
            {
                if (ParseList(val, out list))
                {
                    config.TrustedDomains.AddRange(list);
                    config.Modified.Add(ConfigOption.TrustedDomains);
                    return true;
                }
            }

            if (key == "UnsafeDomains")
            {
                if (ParseList(val, out list))
                {
                    config.UnsafeDomains.AddRange(list);
                    config.Modified.Add(ConfigOption.UnsafeDomains);
                    return true;
                }
            }

            if (key == "UnsafeFiles")
            {
                if (ParseList(val, out list))
                {
                    config.UnsafeFiles.AddRange(list);
                    config.Modified.Add(ConfigOption.UnsafeFiles);
                    return true;
                }
            }

            return false;
        }

        private static bool ParseList(string val, out List<string> ret)
        {
            ret = new List<string>();
            foreach (string line in val.Split('\n'))
            {
                ret.Add(line.Trim());
            }
            return true;
        }

        private static bool ParseBool(string val, out bool ret)
        {
            val = val.ToLower();
            if (val == "yes" || val == "true" || val == "on" || val == "1")
            {
                ret = true;
                return true;
            }

            if (val == "no" || val == "false" || val == "off" || val == "0")
            {
                ret = false;
                return true;
            }

            ret = false;
            return false;
        }

        private static bool ParseInt(string val, out int ret)
        {
            try
            {
                ret = int.Parse(val);
                return true;
            }
            catch
            {
                ret = -1;
                return false;
            }
        }
    }
}
