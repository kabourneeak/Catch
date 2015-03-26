using System.Collections.Generic;
using Catch.Drawable;

namespace Catch.Services
{
    public class CompiledConfig : IConfig
    {
        private readonly IConfig _parentConfig;
        private readonly Dictionary<string, object> _entries = new Dictionary<string, object>();

        public CompiledConfig() : this(null)
        {

        }

        public CompiledConfig(IConfig parentConfig)
        {
            _parentConfig = (_parentConfig == null) ? new EmptyConfig() : parentConfig;

            /*
             * Configuration entries go here!
             * 
             * This IConfig implementation uses the convention that all config keys 
             * are prefixed with the full name of the owning class.
             * 
             * The Add* helper methods ensure that the type you intend to store for
             * a config entry is the type that ultimately gets stored.
             */

            AddFloat(Map.ConfigKeys.TileRadius, 60.0f);
            AddInt(Block.ConfigKeys.BlockSize, 10);
        }

        public int GetInt(string key)
        {
            return HasOwnKey(key) ? (int)_entries[key] : _parentConfig.GetInt(key);
        }

        public int GetInt(string key, int def)
        {
            return HasOwnKey(key) ? (int)_entries[key] : _parentConfig.GetInt(key, def);
        }

        public string GetString(string key)
        {
            return HasOwnKey(key) ? (string)_entries[key] : _parentConfig.GetString(key);
        }

        public string GetString(string key, string def)
        {
            return HasOwnKey(key) ? (string)_entries[key] : _parentConfig.GetString(key, def);
        }

        public float GetFloat(string key)
        {
            return HasOwnKey(key) ? (float)_entries[key] : _parentConfig.GetFloat(key);
        }

        public float GetFloat(string key, float def)
        {
            return HasOwnKey(key) ? (float)_entries[key] : _parentConfig.GetFloat(key, def);
        }

        public double GetDouble(string key)
        {
            return HasOwnKey(key) ? (double)_entries[key] : _parentConfig.GetDouble(key);
        }

        public double GetDouble(string key, double def)
        {
            return HasOwnKey(key) ? (double)_entries[key] : _parentConfig.GetDouble(key, def);
        }

        public bool GetBool(string key)
        {
            return HasOwnKey(key) ? (bool)_entries[key] : _parentConfig.GetBool(key);
        }

        public bool GetBool(string key, bool def)
        {
            return HasOwnKey(key) ? (bool)_entries[key] : _parentConfig.GetBool(key, def);
        }

        public bool HasKey(string key)
        {
            // relies on short-circut eval of || for efficiency
            return HasOwnKey(key) || _parentConfig.HasKey(key);
        }

        private bool HasOwnKey(string key)
        {
            return _entries.ContainsKey(key);
        }

        private void AddInt(string key, int val)
        {
            _entries.Add(key, val);
        }

        private void AddString(string key, string val)
        {
            _entries.Add(key, val);
        }

        private void AddFloat(string key, float val)
        {
            _entries.Add(key, val);
        }

        private void AddDouble(string key, double val)
        {
            _entries.Add(key, val);
        }

        private void AddBool(string key, bool val)
        {
            _entries.Add(key, val);
        }
    }
}
