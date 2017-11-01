using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Catch.Services
{
    /// <summary>
    /// An implementation of IConfig that serves up an already populated dictionary
    /// </summary>
    public class DictionaryConfig : IConfig
    {
        private readonly IConfig _parentConfig;
        private readonly Dictionary<string, string> _dict;

        public DictionaryConfig(Dictionary<string, string> dict, IConfig parent)
        {
            this._dict = new Dictionary<string, string>();
            this._parentConfig = parent;

            // copy over dictionary
            foreach (var entry in dict)
                _dict.Add(entry.Key, entry.Value);

            // resolve any references to other config values. This will consider our own
            // values first, over our parent, and so on, as is usual for config value 
            // resolution
            foreach (var entry in dict)
            {
                if (this.HasKey(entry.Value))
                {
                    _dict[entry.Key] = GetString(entry.Value);
                }
            }
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

        public static DictionaryConfig FromJson(string filename, IConfig parentConfig)
        {
            try
            {
                var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var assetsFolder = appInstalledFolder.GetFolderAsync("Assets")
                    .GetAwaiter()
                    .GetResult();

                var configPath = Path.Combine(assetsFolder.Path, filename);
                var configData = File.ReadAllText(configPath);

                var entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(configData);

                return new DictionaryConfig(entries, parentConfig);
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new IOException($"Could not read config.json", e);
            }
        }
    }
}
