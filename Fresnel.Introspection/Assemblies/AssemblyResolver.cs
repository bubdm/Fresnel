using System;
using System.IO;
using System.Reflection;


namespace Envivo.Fresnel.Core.Assemblies
{
    /// <summary>
    /// Used to resolve unknown assemblies
    /// </summary>
    public class AssemblyResolver
    {
        private readonly string _DllExtension = ".dll";
        private readonly string _ExeExtension = ".exe";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var currentAppDomain = sender as AppDomain;

            var result = this.Resolve(sender, args, currentAppDomain.BaseDirectory);
            if (result == null)
            {
                // Try another path:
                result = this.Resolve(sender, args, currentAppDomain.RelativeSearchPath);
            }

            return result;
        }

        /// <summary>
        /// Returns the assembly for the given ResolveEventArgs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="searchPath">The folder that contains the assembly file</param>
        /// <returns></returns>
        public Assembly Resolve(object sender, ResolveEventArgs args, string searchPath)
        {
            if (string.IsNullOrEmpty(searchPath))
            {
                searchPath = string.Empty;
            }

            var assemblyName = args.Name;
            if (assemblyName.Contains(", Version"))
            {
                // Looks like a strongly nameed assembly, so extract the assembly name:
                assemblyName = assemblyName.Split(',')[0];
            }

            var filePath = args.Name;
            if (File.Exists(filePath) == false)
            {
                // Work out which file contains the assembly:
                filePath = Path.Combine(searchPath, assemblyName);

                if (File.Exists(filePath))
                {
                    // We've got it
                }
                else if (File.Exists(string.Concat(filePath, _DllExtension)))
                {
                    filePath += _DllExtension;
                }
                else if (File.Exists(string.Concat(filePath, _ExeExtension)))
                {
                    filePath += _ExeExtension;
                }
            }

            Assembly result = null;
            if (File.Exists(filePath))
            {
                result = Assembly.LoadFrom(filePath);
            }
            return result;
        }

    }

}
