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

        public int GetInt(string key)
        {
            string val = GetString(key);
            try
            {
                return int.Parse(val);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool GetBool(string key)
        {
            string val = GetString(key).ToLower();
            if (val == "yes" || val == "y" || val == "true" || val == "on")
            {
                return true;
            }
            else if (val == "no" || val == "n" || val == "false" || val == "off")
            {
                return false;
            }
            return false;
        }

        public string GetString(string key)
        {
            if (_options.ContainsKey(key))
            {
                return _options[key];
            }
            if (_defaults.ContainsKey(key))
            {
                return _defaults[key];
            }
            return "";
        }

        public List<string> GetList(string key)
        {
            if (_lists.ContainsKey(key))
            {
                return _lists[key];
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
