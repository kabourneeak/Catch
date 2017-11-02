using System;
using System.Collections;
using System.Collections.Generic;

namespace Catch.Services
{
    /// <summary>
    /// Provides "read-only" configuration data to consumers, although it cannot 
    /// guarantee that the underlying implementation will never vary its results.
    /// 
    /// Implementations should throw a KeyNotFoundException when no entry for the
    /// key can be found, and no default is specified.
    /// 
    /// Default or otherwise, InvalidCastException will be thrown if the entry
    /// requested cannot be cast.
    /// </summary>
    public interface IConfig
    {
        int GetInt(string key);
        int GetInt(string key, int def);

        string GetString(string key);
        string GetString(string key, string def);

        float GetFloat(string key);
        float GetFloat(string key, float def);

        double GetDouble(string key);
        double GetDouble(string key, double def);

        bool GetBool(string key);
        bool GetBool(string key, bool def);

        bool HasKey(string key);

        IEnumerable<KeyValuePair<string, string>> GetOwnEntries();
    }
}
