namespace Catch.Services
{
    /// <summary>
    /// Helper methods for working with configuration entries
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// Create a formatted configuration path name. E.g., creates a string of the form "Core.TileRadius"
        /// to help with making uniformly formatted paths.
        /// </summary>
        /// <param name="family">The family of configuration settings. Typically this is nameof(class)</param>
        /// <param name="entry">The entry, typically this is nameof(public static string). If the substring "Cfg" appears, 
        /// it will be removed</param>
        /// <returns>A formatted configuration path name</returns>
        public static string GetConfigPath(string family, string entry)
        {
            return $"{family}.{entry.Replace("Cfg", "")}";
        }
    }
}
