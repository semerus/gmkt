using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GiraffeStar
{
    public class ConfigAsset : ScriptableObject
    {        
        public List<ConfigItem> Configurations = new List<ConfigItem>();
        
        public Dictionary<string, string> GetConfigs()
        {
            var configs = new Dictionary<string, string>();

            foreach (var item in Configurations)
            {
                if(!item.IsOn) { continue; }
                if(configs.ContainsKey(item.Key)) { continue; }
                configs.Add(item.Key, item.Value);
            }

            return configs;
        }
    }

    [Serializable]
    public class ConfigItem
    {
        public bool IsOn = true;
        public string Key = string.Empty;
        public string Value = string.Empty;
    }
}


