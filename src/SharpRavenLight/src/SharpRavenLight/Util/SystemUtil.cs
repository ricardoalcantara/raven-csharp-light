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
            #if NET_CORE
                return System.Reflection.Assembly
                    .GetExecutingAssembly()
                    .GetReferencedAssemblies()
                    .OrderBy(a => a.Name)
                    .ToDictionary(key => key.Name, value => value.Version.ToString());
            #elif NET45
                var assemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    #if (!net35)
                    .Where(q => !q.IsDynamic)
                    #endif
                    .Select(a => a.GetName())
                    .OrderBy(a => a.Name);

                var dictionary = new Dictionary<string, string>();

                foreach (var assembly in assemblies)
                {
                    if (dictionary.ContainsKey(assembly.Name))
                        continue;

                    dictionary.Add(assembly.Name, assembly.Version.ToString());
                }

                return dictionary;
            #else
                return new Dictionary<string, string>();
            #endif
        }
    }
}
