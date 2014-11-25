using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class ClassHierarchyBuilder
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private TemplateCache _TemplateCache;
        private ClassHierarchyDepthComparer _ClassHierarchyDepthComparer;

        public ClassHierarchyBuilder
        (
            AssemblyReaderMap assemblyReaderMap,
            TemplateCache templateCache,
            ClassTemplate tRootClass
        )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _TemplateCache = templateCache;
            _ClassHierarchyDepthComparer = new ClassHierarchyDepthComparer();
        }

        /// <summary>
        /// Returns a list of Class Templates for the entire tree (i.e. all Super-Classes and all Sub-Classes)
        /// </summary>

        public IEnumerable<ClassTemplate> GetCompleteTree(ClassTemplate tRootClass)
        {
            var results = new List<ClassTemplate>();

            var assemblyReader = _AssemblyReaderMap[tRootClass.RealType];

            //-----

            var superClass = tRootClass.RealType;
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
                // This allows us to test for Interfaces inheritance too:
                if (tClass.RealType.IsDerivedFrom(tRootClass.RealType) == false)
                    continue;

                if (results.Contains(tClass))
                    continue;

                if (tClass.Equals(tRootClass))
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


        public IEnumerable<ClassTemplate> GetSubClasses(ClassTemplate tRootClass, bool includeStartingClass, bool useDeepScan)
        {
            var results = new List<ClassTemplate>();

            if (includeStartingClass)
            {
                results.Add(tRootClass);
            }

            var superClass = tRootClass.RealType;
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

                    if (tClass.Equals(tRootClass))
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


        public IEnumerable<ClassTemplate> GetInterfaces(ClassTemplate tRootClass)
        {
            var results = new List<ClassTemplate>();

            foreach (var iFace in tRootClass.RealType.GetInterfaces())
            {
                if (iFace.IsTrackable() || iFace.IsValueObject())
                {
                    var tInterface = (ClassTemplate)_TemplateCache.GetTemplate(iFace);
                    results.Add(tInterface);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns all Properties from all of the Sub-Classes in the Hierarchy
        /// </summary>


        public IEnumerable<PropertyTemplate> GetProperties(ClassTemplate tRootClass)
        {
            var tProperties = new Dictionary<string, PropertyTemplate>();

            var tSubClasses = this.GetSubClasses(tRootClass, true, true);
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
            return tProperties.Values;
        }

    }
}
