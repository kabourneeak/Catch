using System.Collections.Generic;

namespace Catch.Services
{
    /// <summary>
    /// A very basic implementation of IConfig which does not contain any entries.  
    /// It will throw KeyNotFoundException for every request which does not supply
    /// a default value, otherwise, it always returns the default value.
    /// 
    /// This class is helpful as the "parent config" to a config which does not 
    /// otherwise have a parent so that it does not always need to check for nulls
    /// or handle KeyNotFoundExceptions on its own.
    /// </summary>
    public class EmptyConfig : IConfig
    {
        public int GetInt(string key) => throw new KeyNotFoundException($"No such config key:{key}");

        public int GetInt(string key, int def) => def;

        public string GetString(string key) => throw new KeyNotFoundException($"No such config key:{key}");

        public string GetString(string key, string def) => def;

        public float GetFloat(string key) => throw new KeyNotFoundException($"No such config key:{key}");

        public float GetFloat(string key, float def) => def;

        public double GetDouble(string key) => throw new KeyNotFoundException($"No such config key:{key}");

        public double GetDouble(string key, double def) => def;

        public bool GetBool(string key) => throw new KeyNotFoundException($"No such config key:{key}");

        public bool GetBool(string key, bool def) => def;

        public bool HasKey(string key) => false;
    }
}
