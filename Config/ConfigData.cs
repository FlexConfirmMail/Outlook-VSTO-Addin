using System;
using System.Collections.Generic;

namespace FlexConfirmMail.Config
{
    public class ConfigData
    {
        private Dictionary<string, string> _options;
        private Dictionary<string, List<string>> _lists;
        private Dictionary<string, string> _defaults;

        public ConfigData()
        {
            _options = new Dictionary<string, string>();
            _lists = new Dictionary<string, List<string>>();
            _defaults = ConfigDefault.Get();
        }

        public void AddOption(string key, string val)
        {
            _options[key] = val;
        }

        public void AddList(string key, List<string> val)
        {
            _lists[key] = val;
        }

        public string GetString(string key)
        {
            string val;
            if (_options.TryGetValue(key, out val))
            {
                return val;
            }
            if (_defaults.TryGetValue(key, out val))
            {
                return val;
            }
            return "";
        }

        public int GetInt(string key)
        {
            int val;
            if (TryGetInt(_options, key, out val))
            {
                return val;
            }
            if (TryGetInt(_defaults, key, out val))
            {
                return val;
            }
            return 0;
        }

        private bool TryGetInt(Dictionary<string, string> dict, string key, out int val)
        {
            string raw;
            if (dict.TryGetValue(key, out raw))
            {
                try
                {
                    val = int.Parse(raw);
                    return true;
                }
                catch
                {
                }
            }
            val = 0;
            return false;
        }

        public bool GetBool(string key)
        {
            bool val;
            if (TryGetBool(_options, key, out val))
            {
                return val;
            }
            if (TryGetBool(_defaults, key, out val))
            {
                return val;
            }
            return false;
        }

        private bool TryGetBool(Dictionary<string, string> dict, string key, out bool val)
        {
            string raw;
            if (dict.TryGetValue(key, out raw))
            {
                raw = raw.ToLower();
                if (raw == "yes" || raw == "y" || raw == "true" || raw == "on")
                {
                    val = true;
                    return true;
                }
                if (raw == "no" || raw == "n" || raw == "false" || raw == "off")
                {
                    val = false;
                    return true;
                }
            }
            val = false;
            return false;
        }

        public List<string> GetList(string key)
        {
            List<string> val;
            if (_lists.TryGetValue(key, out val))
            {
                return val;
            }
            return new List<string>();
        }

        // In most cases, we just want to check if certain items is
        // included in the list. Since HashSet can perform that with
        // O(1) time, this interface should be useful.
        public HashSet<string> GetHashSet(string key)
        {
            return new HashSet<string>(GetList(key));
        }

    }
}
