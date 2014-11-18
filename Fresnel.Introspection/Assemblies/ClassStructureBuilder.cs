using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Envivo.Fresnel.Core.Assemblies
{

    /// <summary>
    /// Builds a structure of the Classes within the supplied Assembly
    /// </summary>
    /// <remarks></remarks>
    internal class ClassStructureBuilder
    {

        private readonly Assembly _Assembly;
        private XmlDocument _ClassStructureXml;
        private IsObjectTrackableSpecification _IsTrackableSpecification;

        /// <summary>
        ///
        /// </summary>
        /// <param name="entityAssembly">The Assembly for which the XML structure is to be built</param>
        /// <remarks></remarks>
        internal ClassStructureBuilder(System.Reflection.Assembly assembly)
        {
            _Assembly = assembly;
            _IsTrackableSpecification = new IsObjectTrackableSpecification();
            this.CreateClassStructureFor(assembly);
        }

        /// <summary>
        /// Returns the class structure as an XML document
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        internal XmlDocument GetClassStructureXml()
        {
            return _ClassStructureXml;
        }

        /// <summary>
        /// Creates an XmlDocument of the class structure for the associated Assembly
        /// </summary>
        /// <remarks></remarks>
        private void CreateClassStructureFor(Assembly assembly)
        {
            _ClassStructureXml = new XmlDocument();
            var parentElement = _ClassStructureXml.CreateElement("root");
            _ClassStructureXml.AppendChild(parentElement);

            this.CreateNamespaceNodesFor(assembly);
            this.CreateNodesFor(assembly);

            // Save the file so that we can open it for debugging:
            var outputFolder = Path.Combine(My.Instance.Engine.GetDefaultSavePath(), "Meta");
            if (Directory.Exists(outputFolder) == false)
            {
                Directory.CreateDirectory(outputFolder);
            }

            var xmlPath = Path.Combine(outputFolder, string.Concat(assembly.GetName().Name, ".ClassStructure.XML"));
            _ClassStructureXml.Save(xmlPath);
        }

        #region "XML Namespace structure"

        /// <summary>
        /// Maps the calling assembly's namespace structure into the XML document
        /// </summary>
        /// <remarks></remarks>
        private void CreateNamespaceNodesFor(Assembly domainAssembly)
        {

            var classes = domainAssembly.GetExportedTypes();
            for (var i = 0; i < classes.Length; i++)
            {
                var classType = classes[i];
                if (_IsTrackableSpecification.IsSatisfiedBy(classType).Failed)
                    continue;

                this.CreateNamespaceNodeFor(classType);
            }
        }

        /// <summary>
        /// Maps an individual class Type into the overall namespace structure
        /// </summary>
        /// <param name="classType">The Type that must be inserted into the XML document</param>
        /// <remarks></remarks>
        private void CreateNamespaceNodeFor(Type classType)
        {
            var startNode = _ClassStructureXml.SelectSingleNode("root");

            var namespaceParts = classType.Namespace.Split('.');
            for (var i = 0; i < namespaceParts.Length; i++)
            {
                var namespacePart = namespaceParts[i];

                // Find a node that matches this element:
                var existingNode = startNode.SelectSingleNode(namespacePart);
                if (existingNode == null)
                {
                    // Create a new namespace node:
                    var newNamespace = _ClassStructureXml.CreateElement(namespacePart);
                    newNamespace.SetAttribute("isNamespace", "True");
                    startNode.AppendChild(newNamespace);
                    startNode = newNamespace;
                }
                else
                {
                    // Do nothing:
                    startNode = existingNode;
                }
            }
        }

        #endregion

        #region "XML Class structure"

        /// <summary>
        /// Maps the calling assembly's class/inheritance structure into the XML document
        /// </summary>
        /// <remarks></remarks>
        private void CreateNodesFor(System.Reflection.Assembly domainAssembly)
        {
            var rootNode = _ClassStructureXml.SelectSingleNode("root");

            var classes = domainAssembly.GetExportedTypes();
            for (var i = 0; i < classes.Length; i++)
            {
                if (classes[i].IsTrackable() == false)
                    continue;

                CreateNodesFor(classes[i]);
            }
        }

        /// <summary>
        /// ' .NET BUG? [Type].FullName returns the AssemblyQualifiedName, which is inconsistent with behaviour from .NET 1.1
        /// This function returns a value which retains the original behaviour of the FullName method.
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetClassFullName(Type classType)
        {
            return string.Concat(classType.Namespace, ".", classType.Name);
        }

        /// <summary>
        /// Creates the XML nodes for the given class Type
        /// </summary>
        /// <param name="classType">The class Type that must be inserted into the XML document</param>
        /// <remarks></remarks>
        private List<XmlNode> CreateNodesFor(Type classType)
        {
            var results = new List<XmlNode>();
            var rootNode = _ClassStructureXml.SelectSingleNode("root");

            // Find the namespace node for this class:
            var nameSpacePath = NamespaceToXPath(classType.Namespace);
            var nameSpaceNode = rootNode.SelectSingleNode(NamespaceToXPath(nameSpacePath));

            // HACK 09/01/10
            if (nameSpaceNode == null)
                return results;

            var classFullName = GetClassFullName(classType);
            var existingNode = nameSpaceNode.SelectSingleNode("//" + NamespaceToXml(classFullName));
            if (existingNode != null)
            {
                results.Add(existingNode);
                return results;
            }

            // Get a list of all the types that this class can map to:
            var superTypes = new List<Type>();
            var baseType = classType.BaseType;
            if (baseType != null)
            {
                if (baseType.Assembly.Equals(classType.Assembly))
                {
                    superTypes.Add(classType.BaseType);
                }
            }
            foreach (var iFace in classType.GetInterfaces())
            {
                if (iFace.Assembly.Equals(classType.Assembly) && iFace.IsTrackable())
                {
                    superTypes.Add(iFace);
                }
            }

            // Now get all of the nodes for the super-types:
            var parentNodes = new List<XmlNode>();
            foreach (var superType in superTypes)
            {
                if (superType.IsPublic == false)
                    continue;

                // Find the 'super-class' node (ie the node that maps to the type):
                var superClassName = GetClassFullName(superType);
                var superclassNode = rootNode.SelectSingleNode(NamespaceToXPath(superClassName));
                if (superclassNode == null)
                {
                    // Can't find one, so add a new node (if it's in the same namespace):
                    if (superType.Namespace == classType.Namespace)
                    {
                        parentNodes.AddRange(CreateNodesFor(superType));
                    }
                }
            }

            // And finally create the nodes:
            if (parentNodes.Count == 0)
            {
                // Use the namespace element - it's as good as we can get...
                parentNodes.Add(nameSpaceNode);
            }

            foreach (var parentNode in parentNodes)
            {
                var newNode = createNodeFor(classType, parentNode);
                results.Add(newNode);
            }
            return results;
        }

        private XmlNode createNodeFor(Type classType, XmlNode parentNode)
        {
            var classFullName = GetClassFullName(classType);
            var newNode = _ClassStructureXml.CreateElement(NamespaceToXml(classFullName));

            if (classType.IsAbstract)
            {
                newNode.SetAttribute("isAbstract", "True");
            }
            if (classType.DeclaringType != null)
            {
                newNode.SetAttribute("isInnerClass", "True");
            }
            if (classType.IsClass)
            {
                newNode.SetAttribute("isClass", "True");
            }
            if (classType.IsGenericType)
            {
                newNode.SetAttribute("isGeneric", "True");
            }

            parentNode.AppendChild(newNode);

            return newNode;
        }

        /// <summary>
        /// Converts Namespace values into XPath values
        /// </summary>
        /// <param name="classNamespace">The Namespace to be converted</param>
        /// <returns>The converted Namespace</returns>
        /// <remarks></remarks>
        private static string NamespaceToXPath(string classNamespace)
        {
            var xpath = classNamespace.Replace(".", "/");
            // Take care of nested types:
            xpath = xpath.Replace("+", "/");
            return ConvertGenericsToken(xpath);
        }

        /// <summary>
        /// Converts Namespace values into 'XML friendly' values
        /// </summary>
        /// <param name="classNamespace">The Namespace to be converted</param>
        /// <returns>The converted Namespace</returns>
        /// <remarks>Some namespaces contain '+' characters, which are not acceptable within an XML document.</remarks>
        private static string NamespaceToXml(string classNamespace)
        {
            var xpath = classNamespace.Replace("+", "_");
            return ConvertGenericsToken(xpath);
        }

        /// <summary>
        /// Converts the Generics token into an XML friendly one
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>Generic types have a quote within their name, which is not XML friendly</remarks>
        static internal string ConvertGenericsToken(string value)
        {
            if (value.Contains("`"))
            {
                return value.Replace("`", "_isGeneric_");
            }
            else
            {
                return value.Replace("_isGeneric_", "`");
            }
        }

        #endregion

    }

}
