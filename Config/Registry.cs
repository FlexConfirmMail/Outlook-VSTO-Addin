using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Win32;

namespace FlexConfirmMail
{
    public class RegistryConfig
    {
        public static Dictionary<string, string> ReadDict(string path)
        {
            QueueLogger.Log($"Registry: {path}");
            try
            {
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(path))
                {
                    if (rk != null)
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        foreach (string key in rk.GetValueNames())
                        {
                            string val = GetString(rk, key);
                            if (val != null)
                            {
                                dict[key] = val;
                            }
                        }
                        return dict;
                    }
                    else
                    {
                        QueueLogger.Log($"* Not found");
                    }
                }
            }
            catch (Exception e)
            {
                QueueLogger.Log($"* {e.GetType().Name}");
            }
            return null;
        }

        private static string GetString(RegistryKey rk, string name)
        {
            object o = rk.GetValue(name);
            switch (rk.GetValueKind(name))
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return (string) o;
                case RegistryValueKind.DWord:
                    return ((int) o).ToString();
                case RegistryValueKind.MultiString:
                    return String.Join("\n", (string[]) o);
            }
            return null;
        }
    }
}
