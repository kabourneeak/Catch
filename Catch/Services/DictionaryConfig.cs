using System.Collections.Generic;

namespace Catch.Services
{
    /// <summary>
    /// An implementation of IConfig that serves up an already populated dictionary
    /// </summary>
    public class DictionaryConfig : IConfig
    {
        private readonly IConfig _parentConfig;
        private readonly Dictionary<string, string> _dict;

        public DictionaryConfig(Dictionary<string, string> dict) : this(dict, new EmptyConfig())
        {
            
        }

        public DictionaryConfig(Dictionary<string, string> dict, IConfig parent)
        {
            this._dict = dict;
            this._parentConfig = parent;
        }

        public int GetInt(string key) =>
            _dict.TryGetValue(key, out var val) ? int.Parse(val) : _parentConfig.GetInt(key);

        public int GetInt(string key, int def) =>
            _dict.TryGetValue(key, out var val) ? int.Parse(val) : _parentConfig.GetInt(key, def);

        public string GetString(string key) =>
            _dict.TryGetValue(key, out var val) ? val : _parentConfig.GetString(key);

        public string GetString(string key, string def) =>
            _dict.TryGetValue(key, out var val) ? val : _parentConfig.GetString(key, def);

        public float GetFloat(string key) =>
            _dict.TryGetValue(key, out var val) ? float.Parse(val) : _parentConfig.GetFloat(key);

        public float GetFloat(string key, float def) =>
            _dict.TryGetValue(key, out var val) ? float.Parse(val) : _parentConfig.GetFloat(key, def);

        public double GetDouble(string key) =>
            _dict.TryGetValue(key, out var val) ? double.Parse(val) : _parentConfig.GetDouble(key);

        public double GetDouble(string key, double def) =>
            _dict.TryGetValue(key, out var val) ? double.Parse(val) : _parentConfig.GetDouble(key, def);

        public bool GetBool(string key) =>
            _dict.TryGetValue(key, out var val) ? bool.Parse(val) : _parentConfig.GetBool(key);

        public bool GetBool(string key, bool def) =>
            _dict.TryGetValue(key, out var val) ? bool.Parse(val) : _parentConfig.GetBool(key, def);

        public bool HasKey(string key) =>
            _dict.ContainsKey(key) || _parentConfig.HasKey(key);
    }
}
