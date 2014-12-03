using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    public class NamespaceHierarchyBuilder
    {
        public HierarchyNode BuildTreeFor(Assembly domainAssembly)
        {
            var rootNode = new HierarchyNode();

            this.CreateNamespaceNodesFor(domainAssembly, rootNode);
            this.CreateNodesFor(domainAssembly, rootNode);

            return rootNode;
        }

        public IEnumerable<HierarchyNode> BuildListFor(Assembly domainAssembly)
        {
            var rootNode = this.BuildTreeFor(domainAssembly);

            var results = new List<HierarchyNode>();
            this.FlattenRecursive(rootNode, results);
            return results;
        }

        private void FlattenRecursive(HierarchyNode node, List<HierarchyNode> flatList)
        {
            if (node == null)
                return;

            flatList.Add(node);

            foreach (var childNode in node.Children)
            {
                this.FlattenRecursive(childNode, flatList);
            }
        }

        private void CreateNamespaceNodesFor(Assembly domainAssembly, HierarchyNode rootNode)
        {
            var classes = domainAssembly.GetExportedTypes();
            for (var i = 0; i < classes.Length; i++)
            {
                var classType = classes[i];
                if (!classType.IsTrackable())
                    continue;

                this.CreateNamespaceNodeFor(classType, rootNode);
            }
        }

        /// <summary>
        /// Maps an individual class Type into the overall namespace structure
        /// </summary>
        /// <param name="classType">The Type that must be inserted into the XML document</param>
        /// <remarks></remarks>
        private void CreateNamespaceNodeFor(Type classType, HierarchyNode rootNode)
        {
            var startNode = rootNode;

            var namespaceParts = classType.Namespace.Split('.');
            for (var i = 0; i < namespaceParts.Length; i++)
            {
                var namespacePart = namespaceParts[i];

                // Find a node that matches this element:
                var existingNode = startNode.FindNodeByName(namespacePart);
                if (existingNode == null)
                {
                    // Create a new namespace node:
                    var newNamespace = new HierarchyNode()
                    {
                        Name = namespacePart,
                        IsNamespace = true
                    };
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

        /// <summary>
        /// Maps the calling assembly's class/inheritance structure into the XML document
        /// </summary>
        /// <remarks></remarks>
        private void CreateNodesFor(Assembly domainAssembly, HierarchyNode rootNode)
        {
            var classes = domainAssembly.GetExportedTypes();
            for (var i = 0; i < classes.Length; i++)
            {
                if (classes[i].IsTrackable() == false)
                    continue;

                CreateNodesFor(classes[i], rootNode);
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
        private List<HierarchyNode> CreateNodesFor(Type classType, HierarchyNode rootNode)
        {
            var results = new List<HierarchyNode>();

            // Find the namespace node for this class:
            var nameSpaceNode = rootNode.FindNodeByPath(classType.Namespace);
            if (nameSpaceNode == null)
                return results;

            var classFullName = GetClassFullName(classType);
            var existingNode = nameSpaceNode.FindNodeByPath(classFullName);
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
            var parentNodes = new List<HierarchyNode>();
            foreach (var superType in superTypes)
            {
                if (superType.IsPublic == false)
                    continue;

                // Find the 'super-class' node (ie the node that maps to the type):
                var superClassName = GetClassFullName(superType);
                var superclassNode = rootNode.FindNodeByPath(superClassName);
                if (superclassNode == null)
                {
                    // Can't find one, so add a new node (if it's in the same namespace):
                    if (superType.Namespace == classType.Namespace)
                    {
                        parentNodes.AddRange(CreateNodesFor(superType, rootNode));
                    }
                }
                else
                {
                    parentNodes.Add(superclassNode);
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

        private HierarchyNode createNodeFor(Type classType, HierarchyNode parentNode)
        {
            var newNode = new HierarchyNode()
            {
                Type = classType,
                Name = classType.Name,
                IsAbstract = classType.IsAbstract,
                IsClass = classType.IsClass,
                IsSubClass = classType.BaseType != null,
                IsGeneric = classType.IsGenericType,
            };

            parentNode.AppendChild(newNode);

            return newNode;
        }

    }

    public class HierarchyNode
    {
        private List<HierarchyNode> _Nodes = new List<HierarchyNode>();

        public string Name { get; internal set; }

        public string FQN { get; internal set; }

        public Type Type { get; internal set; }

        public bool IsNamespace { get; internal set; }
        public bool IsAbstract { get; internal set; }
        public bool IsClass { get; internal set; }
        public bool IsSubClass { get; internal set; }
        public bool IsGeneric { get; internal set; }

        public override string ToString()
        {
            return this.Name;
        }

        public HierarchyNode FindNodeByName(string name)
        {
            return this.FindChildNode(this, n => n.Name == name);
        }

        public HierarchyNode FindNodeByPath(string fullPath)
        {
            return this.FindChildNode(this, n => n.FQN == fullPath);
        }

        private HierarchyNode FindChildNode(HierarchyNode startNode, Func<HierarchyNode, bool> predicate)
        {
            if (predicate(startNode))
                return startNode;

            foreach (var child in startNode.Children)
            {
                var match = this.FindChildNode(child, predicate);
                if (match != null)
                    return match;
            }

            return null;
        }

        public IEnumerable<HierarchyNode> Children
        {
            get { return _Nodes; }
        }

        internal void AppendChild(HierarchyNode childToAdd)
        {
            _Nodes.Add(childToAdd);
            childToAdd.FQN = this.FQN.IsEmpty() ?
                                    childToAdd.Name :
                                    string.Concat(this.FQN, ".", childToAdd.Name);
        }
    }

}
