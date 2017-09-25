using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Catch.Services
{
    /// <summary>
    /// An implementation of IConfig that reads its values out of the Assets\config.json file
    /// which is compiled into the software
    /// </summary>
    public class JsonConfig : IConfig
    {
        private readonly IConfig _parentConfig;
        private readonly Dictionary<string, string> _entries;

        public JsonConfig(string jsonConfigFile) : this(jsonConfigFile, new EmptyConfig())
        {
            
        }

        public JsonConfig(string jsonConfigFile, IConfig parentConfig)
        {
            _parentConfig = parentConfig;

            try
            {
                var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var assetsFolder = appInstalledFolder.GetFolderAsync("Assets")
                        .GetAwaiter()
                        .GetResult();

                var configPath = Path.Combine(assetsFolder.Path, jsonConfigFile);
                var configData = File.ReadAllText(configPath);

                _entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(configData);
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

        public int GetInt(string key) =>
            _entries.TryGetValue(key, out var val) ? int.Parse(val) : _parentConfig.GetInt(key);

        public int GetInt(string key, int def) =>
            _entries.TryGetValue(key, out var val) ? int.Parse(val) : _parentConfig.GetInt(key, def);

        public string GetString(string key) =>
            _entries.TryGetValue(key, out var val) ? val : _parentConfig.GetString(key);

        public string GetString(string key, string def) =>
            _entries.TryGetValue(key, out var val) ? val : _parentConfig.GetString(key, def);

        public float GetFloat(string key) => 
            _entries.TryGetValue(key, out var val) ? float.Parse(val) : _parentConfig.GetFloat(key);

        public float GetFloat(string key, float def) => 
            _entries.TryGetValue(key, out var val) ? float.Parse(val) : _parentConfig.GetFloat(key, def);

        public double GetDouble(string key) => 
            _entries.TryGetValue(key, out var val) ? double.Parse(val) : _parentConfig.GetDouble(key);

        public double GetDouble(string key, double def) => 
            _entries.TryGetValue(key, out var val) ? double.Parse(val) : _parentConfig.GetDouble(key, def);

        public bool GetBool(string key) => 
            _entries.TryGetValue(key, out var val) ? bool.Parse(val) : _parentConfig.GetBool(key);

        public bool GetBool(string key, bool def) => 
            _entries.TryGetValue(key, out var val) ? bool.Parse(val) : _parentConfig.GetBool(key, def);

        public bool HasKey(string key) => 
            _entries.ContainsKey(key) || _parentConfig.HasKey(key);
    }
}
