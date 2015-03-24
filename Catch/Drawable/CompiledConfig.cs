using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catch.Drawable
{
    public class CompiledConfig : IConfig
    {
        private readonly IConfig _parentConfig;
        private readonly Dictionary<string, object> _entries = new Dictionary<string, object>();

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

            AddInt(typeof(Hexagon), Hexagon.ConfigKeys.Radius, 60);
        }

        public int GetInt(string category, string name)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (int) _entries[key] : _parentConfig.GetInt(category, name);
        }

        public int GetInt(string category, string name, int def)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (int)_entries[key] : _parentConfig.GetInt(category, name, def);
        }

        public string GetString(string category, string name)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (string)_entries[key] : _parentConfig.GetString(category, name);
        }

        public string GetString(string category, string name, string def)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (string)_entries[key] : _parentConfig.GetString(category, name, def);
        }

        public float GetFloat(string category, string name)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (float)_entries[key] : _parentConfig.GetFloat(category, name);
        }

        public float GetFloat(string category, string name, float def)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (float)_entries[key] : _parentConfig.GetFloat(category, name, def);
        }

        public double GetDouble(string category, string name)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (double)_entries[key] : _parentConfig.GetDouble(category, name);
        }

        public double GetDouble(string category, string name, double def)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (double)_entries[key] : _parentConfig.GetDouble(category, name, def);
        }

        public bool GetBool(string category, string name)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (bool)_entries[key] : _parentConfig.GetBool(category, name);
        }

        public bool GetBool(string category, string name, bool def)
        {
            var key = Join(category, name);
            return HasOwnKey(key) ? (bool)_entries[key] : _parentConfig.GetBool(category, name, def);
        }

        public bool HasKey(string category, string name)
        {
            // relies on short-circut eval of || for efficiency
            return HasOwnKey(category, name) || _parentConfig.HasKey(category, name);
        }

        private string Join(string category, string name)
        {
            return string.Format("{0}.{1}", category, name);
        }

        private string Join(Type type, string name)
        {
            return Join(type.FullName, name);
        }

        private bool HasOwnKey(string category, string name)
        {
            return HasOwnKey(Join(category, name));
        }

        private bool HasOwnKey(string joinedKey)
        {
            return _entries.ContainsKey(joinedKey);
        }

        private void AddInt(Type type, string name, int val)
        {
            _entries.Add(Join(type, name), val);
        }

        private void AddString(Type type, string name, string val)
        {
            _entries.Add(Join(type, name), val);
        }

        private void AddFloat(Type type, string name, float val)
        {
            _entries.Add(Join(type, name), val);
        }

        private void AddDouble(Type type, string name, double val)
        {
            _entries.Add(Join(type, name), val);
        }

        private void AddBool(Type type, string name, bool val)
        {
            _entries.Add(Join(type, name), val);
        }

    }
}
