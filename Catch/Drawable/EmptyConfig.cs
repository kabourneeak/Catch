using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catch.Drawable
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
        public int GetInt(string category, string name)
        {
            throw new KeyNotFoundException(string.Format("No such config key cat:{0} name:{1}", category, name));
        }

        public int GetInt(string category, string name, int def)
        {
            return def;
        }

        public string GetString(string category, string name)
        {
            throw new KeyNotFoundException(string.Format("No such config key cat:{0} name:{1}", category, name));
        }

        public string GetString(string category, string name, string def)
        {
            return def;
        }

        public float GetFloat(string category, string name)
        {
            throw new KeyNotFoundException(string.Format("No such config key cat:{0} name:{1}", category, name));
        }

        public float GetFloat(string category, string name, float def)
        {
            return def;
        }

        public double GetDouble(string category, string name)
        {
            throw new KeyNotFoundException(string.Format("No such config key cat:{0} name:{1}", category, name));
        }

        public double GetDouble(string category, string name, double def)
        {
            return def;
        }

        public bool GetBool(string category, string name)
        {
            throw new KeyNotFoundException(string.Format("No such config key cat:{0} name:{1}", category, name));
        }

        public bool GetBool(string category, string name, bool def)
        {
            return def;
        }

        public bool HasKey(string category, string name)
        {
            return false;
        }
    }
}
