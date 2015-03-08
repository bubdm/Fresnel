using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Envivo.Fresnel.Introspection.Assemblies
{
    /// <summary>
    /// Used to interrogate a .NET assembly for Object classes
    /// </summary>
    /// <remarks>
    /// We've sacrificed some code readability for optimisations
    /// </remarks>
    public class AssemblyReader : IDisposable
    {
        private IDomainClassRegistrar _DomainClassRegistrar;
        private IDomainDependencyRegistrar _DomainDependencyRegistrar;

        private Dictionary<Type, IClassTemplate> _TemplateMap = new Dictionary<Type, IClassTemplate>();
        private XmlDocument _ClassStructureXml = new XmlDocument();

        private AbstractClassTemplateBuilder _AbstractClassTemplateBuilder;

        //public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates and caches Class Templates for the given Assembly
        /// </summary>
        /// <param name="assembly">The Assembly whose details are to be read</param>

        public AssemblyReader
        (
            IDomainClassRegistrar domainClassRegistrar,
            IDomainDependencyRegistrar domainDependencyRegistrar,
            AbstractClassTemplateBuilder abstractClassTemplateBuilder
        )
        {
            _DomainClassRegistrar = domainClassRegistrar;
            _DomainDependencyRegistrar = domainDependencyRegistrar;

            _AbstractClassTemplateBuilder = abstractClassTemplateBuilder;
        }

        /// <summary>
        /// The Assembly associated with this reader
        /// </summary>
        public Assembly Assembly { get; internal set; }

        public AssemblyConfigurationMap ConfigurationMap { get; internal set; }

        public XmlDocsReader XmlDocReader { get; internal set; }

        public bool IsFrameworkAssembly { get; internal set; }

        //public bool AreInfrastructureServicesEnabled { get; private set; }

        /// <summary>
        /// Returns TRUE if Domain Classes were found in this Assembly (as opposed to Infrastructure Service classes)
        /// </summary>
        public bool ContainsDomainClasses
        {
            get { return _TemplateMap.Count > 0; }
        }

        /// <summary>
        /// An XML representation of the class structure of the Domain Assembly
        /// </summary>
        /// <value>An XmlDocument containing the class structure</value>

        public XmlDocument ClassStructureXml
        {
            get { return _ClassStructureXml; }
        }

        /// <summary>
        /// Returns the codebase of the Assembly as a file path (not a URI)
        /// </summary>
        /// <param name="assembly"></param>

        public string GetAssemblyLocation()
        {
            var path = this.Assembly.CodeBase.Replace(@"file:///", string.Empty);
            return path.Replace(@"/", @"\");
        }

        /// <summary>
        /// Returns a Template from the cache for the given object Type. A new Template is created if one does not already exist in the cache.
        /// </summary>
        /// <param name="classType">The Type of the Object</param>
        /// <returns>A Template for the given object Type</returns>
        /// <remarks>
        /// </remarks>
        public IClassTemplate GetTemplate(Type classType)
        {
            if (classType.IsDerivedFrom<ITemplate>())
            {
                throw new ArgumentOutOfRangeException("Type cannot be a Template");
            }

            var tClass = _TemplateMap.TryGetValueOrNull(classType);

            if (tClass == null)
            {
                tClass = CreateAndCacheTemplate(classType);
            }

            return tClass;
        }

        private IClassTemplate CreateAndCacheTemplate(Type classType)
        {
            var classConfiguration = this.ConfigurationMap.GetClassConfiguration(classType);
            var tClass = _AbstractClassTemplateBuilder.CreateTemplate(classType, classConfiguration);
            ((BaseClassTemplate)tClass).AssemblyReader = this;
            _TemplateMap.Add(classType, tClass);
            return tClass;
        }

        public IClassTemplate GetTemplate<TClass>()
            where TClass : class
        {
            return this.GetTemplate(typeof(TClass));
        }

        public IEnumerable<ClassTemplate> GetTemplates()
        {
            var results = _TemplateMap.OfType<ClassTemplate>().ToArray();
            return results;
        }

        /// <summary>
        /// Returns the number of known Templates for the associated Assembly
        /// </summary>
        public int TemplateCount
        {
            get { return _TemplateMap.Count; }
        }

        public override string ToString()
        {
            return this.Assembly.ToString();
        }

        public void PreLoadClassTemplates()
        {
            var publicTypes = this.Assembly.GetExportedTypes();

            InitialiseDomainClasses(publicTypes);
            InitialiseDomainDependencies(publicTypes);
        }

        private void InitialiseDomainDependencies(Type[] publicTypes)
        {
            var dependencyTypes = publicTypes
                                    .Where(t => t.IsFactory() ||
                                                t.IsRepository() ||
                                                t.IsDomainService())
                                    .ToArray();

            _DomainDependencyRegistrar.RegisterTypes(dependencyTypes);
        }

        private void InitialiseDomainClasses(Type[] publicTypes)
        {
            var domainClasses = publicTypes
                                    .Where(t => t.IsTrackable())
                                    .ToArray();

            _DomainClassRegistrar.RegisterTypes(domainClasses);

            foreach (var type in domainClasses)
            {
                var tClass = this.CreateAndCacheTemplate(type);
            }
        }

        public void Dispose()
        {
            this.Assembly = null;
            _TemplateMap = null;
            _ClassStructureXml = null;
            this.XmlDocReader = null;
        }
    }
}