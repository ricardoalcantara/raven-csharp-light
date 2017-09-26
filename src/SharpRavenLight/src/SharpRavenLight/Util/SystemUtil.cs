using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.Util
{
    /// <summary>
    /// Utility class for retreiving system information.
    /// </summary>
    internal class SystemUtil
    {
        /// <summary>
        /// Checks if a string is null or white space
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string arg)
        {
            return string.IsNullOrEmpty(arg) || string.IsNullOrEmpty(arg.Trim());
        }

        /// <summary>
        /// Return all loaded modules.
        /// </summary>
        /// <returns>
        /// All loaded modules.
        /// </returns>
        internal static Dictionary<string, string> GetModules()
        {
            return System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetReferencedAssemblies()
                .OrderBy(a => a.Name)
                .ToDictionary(key => key.Name, value => value.Version.ToString());;
        }
    }
}
