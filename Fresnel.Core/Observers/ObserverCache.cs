using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// Returns Observers for .NET objects & values
    /// </summary>
    public class ObserverCache
    {
        private ConditionalWeakTable<object, ObjectObserver> _ObjectMap = new ConditionalWeakTable<object, ObjectObserver>();
        private Dictionary<Guid, ObjectObserver> _ObjectIdMap = new Dictionary<Guid, ObjectObserver>();
        private Dictionary<object, NonReferenceObserver> _NonReferenceMap = new Dictionary<object, NonReferenceObserver>();
        private Dictionary<ClassTemplate, ObjectObserver> _ServiceObserverMap = new Dictionary<ClassTemplate, ObjectObserver>();

        private TemplateCache _TemplateCache;
        private AbstractObserverBuilder _AbstractObserverBuilder;
        private ObjectIdResolver _ObjectIdResolver;

        public ObserverCache
        (
            TemplateCache templateCache,
            AbstractObserverBuilder abstractObserverBuilder,
            ObjectIdResolver objectIdResolver
        )
        {
            if (templateCache == null)
                throw new ArgumentNullException("templateCache");

            if (abstractObserverBuilder == null)
                throw new ArgumentNullException("abstractObserverBuilder");

            _TemplateCache = templateCache;
            _AbstractObserverBuilder = abstractObserverBuilder;
            _ObjectIdResolver = objectIdResolver;
        }

        public ObjectObserver GetObserverById(Guid id)
        {
            var result = _ObjectIdMap.TryGetValueOrNull(id);
            return result;
        }

        /// <summary>
        /// Returns an ObjectObserver from the cache for the given Domain object
        /// </summary>
        /// <param name="obj"></param>
        public BaseObjectObserver GetObserver(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var objectType = obj.GetType();
            return this.GetObserver(obj, objectType);
        }

        /// <summary>
        /// Returns an Observer from the cache for the given object Type.
        /// </summary>
        /// <param name="obj">The Object to be observed</param>
        /// <param name="objectType">The Type of the Object to be observed</param>
        public BaseObjectObserver GetObserver(object obj, Type objectType)
        {
            var template = _TemplateCache.GetTemplate(objectType);
            var tClass = template as ClassTemplate;
            var tNonRefClass = template as NonReferenceTemplate;

            // If the object has an ID, use it.  Otherwise, use a new ID to cache the observer:
            Guid id = Guid.Empty;
            if (tClass != null)
            {
                // NB: The object might actually return Guid.Empty, hence the check below:
                id = _ObjectIdResolver.TryGetValue(obj, tClass, Guid.Empty);
            }
            if (id == Guid.Empty)
            {
                // If the object can't provide an ID, we'll assign it ourselves:
                id = Guid.NewGuid();
            }

            var result = tClass != null ?
                        this.GetCachedObserver(id, obj, tClass) :
                        this.GetCachedObserver(id, obj, tNonRefClass);

            if (result == null)
            {
                result = tClass != null ?
                        this.CreateAndCacheObserver(id, obj, tClass) :
                        this.CreateAndCacheObserver(id, obj, tNonRefClass);
            }
            else
            {
                //Debug.WriteLine(string.Concat("Found Observer for ", result.Template.Name, " with ID ", result.ID));
            }

            return result;
        }

        public BaseObjectObserver GetValueObserver(string value, Type valueType)
        {
            BaseObjectObserver oValue = null;

            var isNonReference = valueType.IsNonReference();
            var isNullable = valueType.IsNullableType();

            if (valueType.IsEnum)
            {
                var typedValue = Enum.Parse(valueType, value, true);
                oValue = this.GetObserver(typedValue, valueType);
            }
            else if (isNonReference &&
                     isNullable &&
                     valueType.IsNotTypeOf<string>() &&
                     value == null)
            {
                var targetType = valueType.GetInnerType();
                oValue = this.GetObserver(value, targetType);
            }
            else if (valueType.IsNonReference() &&
                    value == null)
            {
                throw new CoreException("The given value is not allowed");
            }
            else if (isNonReference &&
                     isNullable &&
                     valueType.IsNotTypeOf<string>() &&
                     value != null)
            {
                var targetType = valueType.GetInnerType();
                var typedValue = Convert.ChangeType(value, targetType);
                oValue = this.GetObserver(typedValue, targetType);
            }
            else
            {
                var targetType = valueType;
                var typedValue = value != null ?
                                Convert.ChangeType(value, targetType) :
                                null;
                oValue = this.GetObserver(typedValue, targetType);
            }

            return oValue;
        }

        public ObjectObserver GetServiceObserver(Type domainServiceClass)
        {
            var tDomainService = _TemplateCache.GetTemplate(domainServiceClass) as ClassTemplate;

            var result = _ServiceObserverMap.TryGetValueOrNull(tDomainService);
            if (result == null)
            {
                var dummyObject = new object();
                result = this.CreateAndCacheObserver(Guid.NewGuid(), dummyObject, tDomainService) as ObjectObserver;
                result.IsPinned = true;
                _ServiceObserverMap[tDomainService] = result;
            }

            return result;
        }

        private BaseObjectObserver GetCachedObserver(Guid objectId, object obj, ClassTemplate tClass)
        {
            BaseObjectObserver result = null;

            if (obj == null)
                return null;

            if (tClass == null)
                return null;

            if (objectId != Guid.Empty)
            {
                result = _ObjectIdMap.TryGetValueOrNull(objectId);
            }

            if (result == null)
            {
                ObjectObserver match = null;
                _ObjectMap.TryGetValue(obj, out match);
                result = match;
            }

            return result;
        }

        private BaseObjectObserver GetCachedObserver(Guid objectId, object obj, NonReferenceTemplate tNonRefClass)
        {
            BaseObjectObserver result = null;

            if (obj == null)
                return null;

            if (tNonRefClass == null)
                return null;

            result = _NonReferenceMap.TryGetValueOrNull(obj);
            return result;
        }

        private BaseObjectObserver CreateAndCacheObserver(Guid objectId, object obj, ClassTemplate tClass)
        {
            if (tClass == null)
                return null;

            //Debug.WriteLine(string.Concat("Creating Observer for ", tClass.Name, " with hash code ", obj.GetHashCode()));

            var observer = _AbstractObserverBuilder.BuildFor(obj, tClass);
            var oObject = observer as ObjectObserver;
            if (oObject != null)
            {
                _ObjectMap.Add(obj, oObject);

                ReplaceInvalidKeyWithValidKey(oObject, objectId);

                _ObjectIdMap.Add(objectId, oObject);

                //MergeObjectsWithSameId(obj, oObject);
            }

            return observer;
        }

        private BaseObjectObserver CreateAndCacheObserver(Guid objectId, object obj, NonReferenceTemplate tNonRefClass)
        {
            if (tNonRefClass == null)
                return null;

            var result = _AbstractObserverBuilder.BuildFor(obj, tNonRefClass.RealType);

            if (obj != null)
            {
                _NonReferenceMap.Add(obj, (NonReferenceObserver)result);
            }

            return result;
        }

        private void ReplaceInvalidKeyWithValidKey(ObjectObserver oObject, Guid id)
        {
            try
            {
                var obj = oObject.RealObject;
                var tClass = oObject.Template;

                if (tClass.IdProperty != null)
                {
                    tClass.IdProperty.SetField(obj, id);
                }
            }
            finally
            {
                // Make sure the Observer is tied to the Object:
                oObject.ID = id;
            }
        }

        //private void MergeObjectsWithSameId(object obj, ObjectObserver oObject)
        //{
        //    if (object.ReferenceEquals(obj, oObject.RealObject))
        //        return;

        //    // We've been given a *different* Object with the same ID??!!

        //    System.Diagnostics.Debug.WriteLine("ReferenceEquals failed for " + oObject.DebugID);

        //    //// We should update the cached Object from the given instance.
        //    //// Note that we don't just replace the Object, as that would break
        //    //// existing references to other Domain Objects in the graph:
        //    //if (oParent.IsCollection)
        //    //{
        //    //    _ObjectMerger.MergeCollections(obj, oParent.RealObject);
        //    //}
        //    //else
        //    //{
        //    //    _ObjectMerger.MergeValues(obj, oParent.RealObject);
        //    //}
        //}

        public IEnumerable<ObjectObserver> GetAllObservers()
        {
            return _ObjectIdMap.Values;
        }

        public void ScanForChanges()
        {
            foreach (var oObject in this.GetAllObservers())
            {
                oObject.ChangeTracker.DetectChanges();
            }
        }

        public void CleanUp()
        {
            var disposedIDs = new List<Guid>();

            foreach (var oObject in _ObjectIdMap.Values)
            {
                if (oObject.IsPinned)
                    continue;

                if (oObject.RealObject != null)
                {
                    _ObjectMap.Remove(oObject.RealObject);
                }
                oObject.DisposeSafely();
                disposedIDs.Add(oObject.ID);
            }

            foreach (var id in disposedIDs)
            {
                _ObjectIdMap.Remove(id);
            }

            // Now take care of non-reference values:
            foreach (var oNonRefObject in _NonReferenceMap.Values)
            {
                oNonRefObject.DisposeSafely();
            }

            _NonReferenceMap.Clear();
        }
    }
}