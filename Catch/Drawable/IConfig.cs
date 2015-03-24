using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catch.Drawable
{
    /// <summary>
    /// Provides "read-only" configuration data to consumers, although it cannot 
    /// guarantee that the underlying implementation will never vary its results.
    /// 
    /// Implementations should throw a KeyNotFoundException when no entry for the
    /// category/name combination can be found, and no default is specified.
    /// 
    /// Default or otherwise, InvalidCastException will be thrown if the entry
    /// requested cannot be cast.
    /// </summary>
    public interface IConfig
    {
        int GetInt(string category, string name);
        int GetInt(string category, string name, int def);

        string GetString(string category, string name);
        string GetString(string category, string name, string def);

        float GetFloat(string category, string name);
        float GetFloat(string category, string name, float def);

        double GetDouble(string category, string name);
        double GetDouble(string category, string name, double def);

        bool GetBool(string category, string name);
        bool GetBool(string category, string name, bool def);

        bool HasKey(string category, string name);
    }
}
