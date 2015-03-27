using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            DynamicMethodBuilder dynamicMethodBuilder,
            FieldInfoMapBuilder fieldInfoMapBuilder,
            PropertyTemplateMapBuilder propertyTemplateMapBuilder,
            MethodTemplateMapBuilder methodTemplateMapBuilder,
            StaticMethodTemplateMapBuilder staticMethodTemplateMapBuilder,
            TrackingPropertiesIdentifier trackingPropertiesIdentifier,
            TemplateCache templateCache
        )
            : base(dynamicMethodBuilder,
                    fieldInfoMapBuilder,
                    propertyTemplateMapBuilder,
                    methodTemplateMapBuilder,
                    staticMethodTemplateMapBuilder,
                    trackingPropertiesIdentifier)
        {
            _TemplateCache = templateCache;

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

        internal override void FinaliseConstruction()
        {
            base.FinaliseConstruction();

            this.IsDictionary = this.RealType.IsDerivedFrom(TypeExtensions.IGenericDictionary);

            // We should also have a friendly name than "IList`1":
            if (this.Name.Contains("`"))
            {
                this.FriendlyName = string.Concat("Collection of '", this.ElementType.Name.CreateFriendlyName(), "'");
            }
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
        [JsonIgnore]
        public Type ElementType { get; internal set; }

        [JsonIgnore]
        public ClassTemplate InnerClass
        {
            get { return _InnerClass.Value; }
        }

        private ClassTemplate DetermineInnerClass()
        {
            var result = _TemplateCache.GetTemplate(this.ElementType) as ClassTemplate;
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
        /// Returns TRUE if the Collection has any Add() methods
        /// </summary>
        public bool HasAddMethods
        {
            get { return _AddMethods.Value.Any(); }
        }
        
        /// <summary>
        /// Returns TRUE if the Collection has any Remove() methods
        /// </summary>
        public bool HasRemoveMethods
        {
            get { return _RemoveMethods.Value.Any(); }
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

        ///// <summary>
        ///// Returns an Enumerator of the given List
        ///// </summary>
        //internal IEnumerable GetEnumerator(object collection, Type realCollectionType)
        //{
        //    IEnumerable enumerable = null;
        //    if (collection is IEnumerable)
        //    {
        //        enumerable = ((IEnumerable)collection);
        //    }
        //    else if (realCollectionType.IsDerivedFrom(TypeExtensions.IGenericEnumerable))
        //    {
        //        var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
        //        enumerable = (IEnumerator)TypeExtensions.IGenericEnumerable.InvokeMember("GetEnumerator", flags, null, collection, null);
        //    }

        //    enumerable.

        //    throw new FresnelException("The given list does not expose GetEnumerator");
        //}

        /// <summary>
        /// Returns the number of items in the given list
        /// </summary>
        internal int Count(object collection)
        {
            var enumerable = collection as IEnumerable;
            if (enumerable == null)
                return 0;

            return enumerable
                    .Cast<object>()
                    .Count();

            //var tCountProp = this.Properties.TryGetValueOrNull("Count");
            //if (tCountProp != null)
            //{
            //    return (int)tCountProp.GetProperty(collection);
            //}
            //else
            //{
            //    var enumerator = ((IEnumerable)collection).GetEnumerator();
            //    var count = 0;
            //    while (enumerator.MoveNext())
            //    {
            //        count += 1;
            //    }

            //    return count;
            //}
        }

        /// <summary>
        /// Returns TRUE if the given list contains the given item
        /// </summary>
        internal bool Contains(object collection, object item, Type realItemType)
        {
            var enumerable = collection as IEnumerable;
            if (enumerable == null)
                return false;

            return enumerable
                    .Cast<object>()
                    .Contains(item);

            //var expectedItemType = this.ElementType;
            //if (!realItemType.IsDerivedFrom(expectedItemType))
            //    return false;

            //var tContainsMethod = this.Methods.TryGetValueOrNull("Contains");
            //if (tContainsMethod != null)
            //{
            //    var args = new object[] { item };
            //    return (bool)tContainsMethod.Invoke(list, args);
            //}
            //else
            //{
            //    // Brute force scan:
            //    var enumerator = ((IEnumerable)list).GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        if (object.Equals(enumerator.Current, item))
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //}
        }
    }
}