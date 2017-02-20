using System;
using System.Collections.Generic;
using System.Linq;

namespace Catch.Services
{
    public static class EnumUtils
    {
        public static List<T> GetEnumAsList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
