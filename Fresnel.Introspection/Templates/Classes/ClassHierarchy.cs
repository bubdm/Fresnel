using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// The hierarchy of Classes that descend from the associated Class
    /// </summary>

    public class ClassHierarchy
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private TemplateCache _TemplateCache;
        private ClassTemplate _tRootClass;
        private ClassHierarchyDepthComparer _ClassHierarchyDepthComparer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="tRootClass">The ClassTemplate that this hierarchy belongs to</param>

        internal ClassHierarchy(AssemblyReaderMap assemblyReaderMap,
                                TemplateCache templateCache,
                                ClassTemplate tRootClass)
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _TemplateCache = templateCache;
            _tRootClass = tRootClass;
            _ClassHierarchyDepthComparer = new ClassHierarchyDepthComparer();
        }

        /// <summary>
        /// Returns a list of Class Templates for the entire tree (i.e. all Super-Classes and all Sub-Classes)
        /// </summary>

        public IEnumerable<ClassTemplate> GetCompleteTree()
        {
            var results = new List<ClassTemplate>();

            var assemblyReader = _AssemblyReaderMap[_tRootClass.RealType];

            //-----

            var superClass = _tRootClass.RealType;
            while (superClass != null)
            {
                var tClass = assemblyReader.GetTemplate(superClass) as ClassTemplate;
                if (tClass != null)
                {
                    results.Add(tClass);
                }

                superClass = superClass.BaseType;
            }

            //-----

            foreach (var tClass in assemblyReader.GetTemplates())
            {
                // Note that we're using IsAssignableFrom() instead of IsSubClassOf().
                // This allows us to test for Interfaces inheritance too:
                if (tClass.RealType.IsDerivedFrom(_tRootClass.RealType) == false)
                    continue;

                if (results.Contains(tClass))
                    continue;

                if (tClass.Equals(_tRootClass))
                    continue;

                results.Add(tClass);
            }

            //-----

            if (results.Count > 1)
            {
                // Sort the results by inheritance depth order 
                // (i.e. super classes at the top, sub classes at the bottom):
                results.Sort(_ClassHierarchyDepthComparer);
            }

            return results;
        }


        /// <summary>
        /// Returns all Class Templates for all Sub-Classes
        /// </summary>
        /// <param name="includeStartingClass"></param>
        /// <param name="useDeepScan"></param>


        public IEnumerable<ClassTemplate> GetSubClasses(bool includeStartingClass, bool useDeepScan)
        {
            var results = new List<ClassTemplate>();

            if (includeStartingClass)
            {
                results.Add(_tRootClass);
            }

            var superClass = _tRootClass.RealType;
            var assemblyReader = _AssemblyReaderMap[superClass];

            // We're using two separate loops for optimsation.
            // Placing the IF statement inside the loop will be a bit slower.
            if (useDeepScan)
            {
                foreach (var tClass in assemblyReader.GetTemplates())
                {
                    // This allows us to test for Interfaces inheritance too:
                    if (tClass.RealType.IsDerivedFrom(superClass) == false)
                        continue;

                    if (results.Contains(tClass))
                        continue;

                    if (tClass.Equals(_tRootClass))
                        continue;

                    results.Add(tClass);
                }
            }
            else
            {
                foreach (var tClass in assemblyReader.GetTemplates())
                {
                    if (tClass.RealType.BaseType != superClass)
                        continue;

                    if (results.Contains(tClass))
                        continue;

                    results.Add(tClass);
                }
            }

            //-----

            if (results.Count > 1)
            {
                // Sort the results by inheritance depth order 
                // (i.e. super classes at the top, sub classes at the bottom):
                results.Sort(_ClassHierarchyDepthComparer);
            }

            return results;
        }

        /// <summary>
        /// Returns all Class Templates for all the interfaces that this class implements
        /// </summary>


        public IEnumerable<ClassTemplate> GetInterfaces()
        {
            var results = new List<ClassTemplate>();

            foreach (var iFace in _tRootClass.RealType.GetInterfaces())
            {
                if (iFace.IsTrackable() || iFace.IsValueObject())
                {
                    results.Add((ClassTemplate)_TemplateCache.GetTemplate(iFace));
                }
            }

            return results;
        }

        /// <summary>
        /// Returns all Properties from all of the Sub-Classes in the Hierarchy
        /// </summary>


        public IEnumerable<PropertyTemplate> GetProperties()
        {
            var tProperties = new Dictionary<string, PropertyTemplate>();

            var tSubClasses = this.GetSubClasses(true, true);
            foreach (var tSubClass in tSubClasses)
            {
                foreach (var tProperty in tSubClass.Properties.Values)
                {
                    // Filter out any duplicates:
                    if (tProperties.DoesNotContain(tProperty.Name))
                    {
                        tProperties.Add(tProperty.Name, tProperty);
                    }
                }
            }

            // Send back the results as a simple generic List:
            return new List<PropertyTemplate>(tProperties.Values);
        }

    }
}
