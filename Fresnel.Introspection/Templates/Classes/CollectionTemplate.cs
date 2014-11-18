using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents a Collection/List Class
    /// </summary>
    /// <remarks>
    /// NB. Although a Collection is strongly typed, it may have multiple overloaded 'Add' and 'Remove' methods.
    ///     We need to recognise and invoke these methods if the correct type of object is passed to them.
    /// </remarks>
    public class CollectionTemplate : ClassTemplate
    {
        private TemplateCache _TemplateCache;
        private Lazy<ClassTemplate> _InnerClass;
        private Lazy<List<MethodTemplate>> _AddMethods;
        private Lazy<List<MethodTemplate>> _RemoveMethods;

        public CollectionTemplate
        (
            FieldInfoMapBuilder fieldInfoMapBuilder,
            PropertyTemplateMapBuilder propertyTemplateMapBuilder,
            MethodTemplateMapBuilder methodTemplateMapBuilder,
            StaticMethodTemplateMapBuilder staticMethodTemplateMapBuilder,
            TrackingPropertiesIdentifier trackingPropertiesIdentifier,
            TemplateCache templateCache
        )
            : base(fieldInfoMapBuilder,
                    propertyTemplateMapBuilder,
                    methodTemplateMapBuilder,
                    staticMethodTemplateMapBuilder,
                    trackingPropertiesIdentifier)
        {
            _TemplateCache = templateCache;
        }

        internal override void FinaliseConstruction()
        {
            base.FinaliseConstruction();

            this.IsDictionary = this.RealObjectType.IsDerivedFrom(TypeExtensions.IGenericDictionary);

            // We should also have a friendly name than "IList`1":
            if (this.Name.Contains("`"))
            {
                this.FriendlyName = string.Concat("Collection of '", this.ElementType.Name.CreateFriendlyName(), "'");
            }

            _InnerClass = new Lazy<ClassTemplate>(
                                    () => this.DetermineInnerClass(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _AddMethods = new Lazy<List<MethodTemplate>>(
                                    () => this.DetermineAddMethods(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _RemoveMethods = new Lazy<List<MethodTemplate>>(
                                    () => this.DetermineRemoveMethods(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Returns TRUE if the collection contains Objects (i.e. NOT non-reference types)
        /// </summary>
        public bool ContainsObjectItems
        {
            get
            {
                return this.ElementType == null ?
                       false :
                       this.ElementType.IsNonReference() == false;
            }
        }

        public override bool IsPersistable
        {
            get { return this.ContainsObjectItems && this.InnerClass.IsPersistable; }
        }

        /// <summary>
        /// Returns TRUE if the collection inherits from IDictionary
        /// </summary>
        public bool IsDictionary { get; private set; }

        /// <summary>
        /// The type of Item contained in the collection
        /// </summary>
        public Type ElementType { get; internal set; }

        public ClassTemplate InnerClass { get { return _InnerClass.Value; } }

        private ClassTemplate DetermineInnerClass()
        {
            var result = (ClassTemplate)_TemplateCache.GetTemplate(this.RealObjectType);
            return result;
        }

        private List<MethodTemplate> DetermineAddMethods()
        {
            var results = this.Methods.Values.Where(m => m.Parameters.Count == 1 &&
                                                         m.MethodInfo.Name == "Add");
            return results.ToList();
        }

        private List<MethodTemplate> DetermineRemoveMethods()
        {
            var results = this.Methods.Values.Where(m => m.Parameters.Count == 1 &&
                                                         m.MethodInfo.Name == "Remove");
            return results.ToList();
        }

        public MethodTemplate GetAddMethodThatAccepts(Type realItemType)
        {
            var result = _AddMethods.Value.SingleOrDefault(m => m.Parameters.Values.First()
                                                            .CanAccept(realItemType));
            return result;
        }

        public MethodTemplate GetRemoveMethodThatAccepts(Type realItemType)
        {
            var result = _RemoveMethods.Value.SingleOrDefault(m => m.Parameters.Values.First()
                                                            .CanAccept(realItemType));
            return result;
        }

        /// <summary>
        /// Determines if the Collection has an 'Add()' method which accepts the given ElementType
        /// </summary>
        internal bool ContainsAddFor(Type realItemType)
        {
            var method = this.GetAddMethodThatAccepts(realItemType);
            return method != null;
        }

        /// <summary>
        /// Determines if the Collection has a 'Remove()' method which accepts the given ElementType
        /// </summary>
        internal bool ContainsRemoveFor(Type realItemType)
        {
            var method = this.GetRemoveMethodThatAccepts(realItemType);
            return method != null;
        }

        /// <summary>
        /// Executes the Add method on the given Collection with the given args
        /// </summary>
        /// <param name="targetCollection"></param>
        /// <param name="args"></param>
        
        public object Add(object targetCollection, object itemToAdd, Type realItemType)
        {
            var tMethod = GetAddMethodThatAccepts(realItemType);
            if (tMethod != null)
            {
                var result = tMethod.Invoke(targetCollection, new object[] { itemToAdd });
                return result;
            }

            return null;
        }

        /// <summary>
        /// Executes the Remove method on the given Collection with the given args
        /// </summary>
        /// <param name="targetCollection"></param>
        /// <param name="args"></param>
        public bool Remove(object targetCollection, object itemToRemove, Type realItemType)
        {
            var tMethod = GetRemoveMethodThatAccepts(realItemType);
            if (tMethod != null)
            {
                var result = tMethod.Invoke(targetCollection, new object[] { itemToRemove });
                if (result is bool)
                {
                    return (bool)result;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an Enumerator of the given List
        /// </summary>
        
        /// <remarks>Returns an IEnumerator regardless of whether it's a Collection, List, a Dictionary</remarks>
        internal IEnumerator GetEnumerator(object collection, Type realCollectionType)
        {
            if (collection is IEnumerable)
            {
                return ((IEnumerable)collection).GetEnumerator();
            }
            else if (realCollectionType.IsDerivedFrom(TypeExtensions.IGenericEnumerable))
            {
                var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
                return (IEnumerator)TypeExtensions.IGenericEnumerable.InvokeMember("GetEnumerator", flags, null, collection, null);
            }

            throw new FresnelException("The given list does not expose GetEnumerator");
        }

        internal void Clear(object collection)
        {
            var tClearMethod = this.Methods.TryGetValueOrNull("Clear");
            if (tClearMethod != null)
            {
                tClearMethod.Invoke(collection, null);
            }
        }

        /// <summary>
        /// Returns the number of items in the given list
        /// </summary>
        /// <param name="collectioresultsparam>
        
        internal int Count(object collection)
        {
            if (collection == null)
                return 0;

            var tCountProp = this.Properties.TryGetValueOrNull("Count");
            if (tCountProp != null)
            {
                return (int)tCountProp.GetProperty(collection);
            }
            else
            {
                var enumerator = ((IEnumerable)collection).GetEnumerator();
                var count = 0;
                while (enumerator.MoveNext())
                {
                    count += 1;
                }

                return count;
            }
        }

        /// <summary>
        /// Returns TRUE if the given list contains the given item
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        
        internal bool Contains(object list, object item, Type realItemType)
        {
            if (list == null)
                return false;

            var expectedItemType = this.ElementType;
            if (!realItemType.IsDerivedFrom(expectedItemType))
                return false;

            var tContainsMethod = this.Methods.TryGetValueOrNull("Contains");
            if (tContainsMethod != null)
            {
                var args = new object[] { item };
                return (bool)tContainsMethod.Invoke(list, args);
            }
            else
            {
                // Brute force scan:
                var enumerator = ((IEnumerable)list).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (object.Equals(enumerator.Current, item))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

    }

}
