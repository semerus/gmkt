using System.Collections.Generic;
using UnityEngine;
using GiraffeStar;

public class Config {

    static Dictionary<string, string> config = new Dictionary<string, string>();
    static bool isInitialized;

    public static void Init()
    {
        if(isInitialized) { return; }

        var configAsset = Resources.Load<ConfigAsset>("Config/Config");
        if(configAsset == null) { return; }

        config = configAsset.GetConfigs();

        isInitialized = true;
    }

    public static bool GetBoolOrDefault(string key, bool @default)
    {
        var value = string.Empty;
        if(config.TryGetValue(key, out value))
        {
            if (value.Equals("True"))
            {
                return true;
            }
            else if (value.Equals("False"))
            {
                return false;
            }
        }

        return @default;
    }

    public static string GetString(string key)
    {
        var value = string.Empty;
        if(config.TryGetValue(key, out value))
        {
            return value;
        }
        return string.Empty;
    }
}
