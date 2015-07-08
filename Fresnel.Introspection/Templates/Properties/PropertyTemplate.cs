using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// A Template that represents a Property in a .NET class
    /// </summary>

    public class PropertyTemplate : BaseMemberTemplate, ISettableMemberTemplate
    {
        public AllowedOperationsAttribute _AllowedOperations;
        public PersistableAttribute _Persistable;

        private Lazy<IClassTemplate> _InnerClass;

        private DynamicMethodBuilder _DynamicMethodBuilder;
        private TemplateCache _TemplateCache;
        private BackingFieldIdentifier _BackingFieldIdentifier;

        private Lazy<FieldInfo> _BackingField;
        private Lazy<RapidGet> _RapidFieldGet;
        private Lazy<RapidSet> _RapidFieldSet;
        private Lazy<RapidGet> _RapidPropGet;
        private Lazy<RapidSet> _RapidPropSet;

        public PropertyTemplate
        (
            DynamicMethodBuilder dynamicMethodBuilder,
            TemplateCache templateCache,
            BackingFieldIdentifier backingFieldIdentifier
        )
        {
            _DynamicMethodBuilder = dynamicMethodBuilder;
            _TemplateCache = templateCache;
            _BackingFieldIdentifier = backingFieldIdentifier;

            _BackingField = new Lazy<FieldInfo>(
                                () => _BackingFieldIdentifier.GetBackingField(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _InnerClass = new Lazy<IClassTemplate>(
                                () => this.DetermineInnerClass(),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _RapidFieldGet = new Lazy<RapidGet>(
                                () => _DynamicMethodBuilder.BuildGetHandler(this.BackingField),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _RapidFieldSet = new Lazy<RapidSet>(
                                () => _DynamicMethodBuilder.BuildSetHandler(this.BackingField),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _RapidPropGet = new Lazy<RapidGet>(
                                () => _DynamicMethodBuilder.BuildGetHandler(this.PropertyInfo),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _RapidPropSet = new Lazy<RapidSet>(
                                () => _DynamicMethodBuilder.BuildSetHandler(this.PropertyInfo),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _XmlComments = new Lazy<XmlComments>(
                                () => this.AssemblyReader.XmlDocReader.GetXmlCommentsFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        internal override void FinaliseConstruction()
        {
            _AllowedOperations = this.Attributes.Get<AllowedOperationsAttribute>();
            _Persistable = this.Attributes.Get<PersistableAttribute>();

            base.FinaliseConstruction();
        }

        private IClassTemplate DetermineInnerClass()
        {
            var result = _TemplateCache.GetTemplate(this.PropertyType);
            return result;
        }

        /// <summary>
        /// Returns the value of the property using the property getter
        /// </summary>
        public object GetProperty(object obj)
        {
            var rapidGet = _RapidPropGet.Value;

            if (rapidGet != null)
            {
                return rapidGet.Invoke(obj);
            }
            else if (this.PropertyInfo.CanRead)
            {
                // Fallback is using standard Reflection:
                return this.PropertyInfo.GetValue(obj, null);
            }
            else
            {
                throw new MethodAccessException("Unable to identify Property Getter for " + this.FullName);
            }
        }

        /// <summary>
        /// Gets the value of the property using the backing field
        /// </summary>
        public object GetField(object obj)
        {
            var rapidGet = _RapidFieldGet.Value;

            if (rapidGet != null)
            {
                return rapidGet.Invoke(obj);
            }
            else if (this.BackingField != null)
            {
                // Fallback is using standard Reflection:
                return this.BackingField.GetValue(obj);
            }
            else
            {
                throw new MethodAccessException("Unable to identify Backing Field for " + this.FullName);
            }
        }

        /// <summary>
        /// Sets the value of the property using the property setter
        /// </summary>
        public void SetProperty(object obj, object value)
        {
            var rapidSet = _RapidPropSet.Value;

            if (rapidSet != null)
            {
                rapidSet.Invoke(obj, value);
            }
            else if (this.PropertyInfo.CanWrite)
            {
                // Fallback is using standard Reflection:
                this.PropertyInfo.SetValue(obj, value, null);
            }
            else
            {
                throw new MethodAccessException("Unable to identify Property Setter for " + this.FullName);
            }
        }

        /// <summary>
        /// Sets the value of the property using the backing field
        /// </summary>
        public void SetField(object obj, object value)
        {
            var rapidSet = _RapidFieldSet.Value;

            if (rapidSet != null)
            {
                rapidSet.Invoke(obj, value);
            }
            else if (this.BackingField != null)
            {
                // Fallback is using standard Reflection:
                this.BackingField.SetValue(obj, value);
            }
            else
            {
                throw new MethodAccessException("Unable to identify Backing Field for " + this.FullName);
            }
        }

        /// <summary>
        /// The reflected backing field for this Property
        /// </summary>
        [JsonIgnore]
        public FieldInfo BackingField { get { return _BackingField.Value; } }

        /// <summary>
        /// The reflected Property
        /// </summary>
        [JsonIgnore]
        public PropertyInfo PropertyInfo { get; internal set; }

        /// <summary>
        /// Returns the actual type of the Property (e.g. if it's a Nullable type)
        /// </summary>
        [JsonIgnore]
        public Type PropertyType { get; internal set; }

        public Type ValueType
        {
            get { return this.PropertyType; }
        }

        /// <summary>
        /// Returns the Template of the declared type of the Property.
        /// </summary>
        [JsonIgnore]
        public IClassTemplate InnerClass
        {
            get { return _InnerClass.Value; }
        }

        /// <summary>
        /// Determines if the Property is an auto-property (ie. doesn't contain any logic)
        /// </summary>
        public bool IsAutoProperty
        {
            get
            {
                return _BackingField.Value != null &&
                       _BackingField.Value.Name.EndsWith(BackingFieldIdentifier.AutoPropertyFieldSuffix);
            }
        }

        /// <summary>
        /// Determines if the value of the Property is a Reference object
        /// </summary>
        public bool IsReferenceType { get; internal set; }

        /// <summary>
        /// Determines if the value of the Property is a Non-Reference value
        /// </summary>
        /// <value>True = The value of the Property is a Non-Reference value</value>
        public bool IsNonReference { get; internal set; }

        /// <summary>
        /// Determines if the value of the Property is a Domain Object
        /// </summary>
        public bool IsDomainObject { get; internal set; }

        /// <summary>
        /// Determines if the value of the Property is a ValueObject
        /// </summary>
        public bool IsValueObject { get; internal set; }

        /// <summary>
        /// Determines if the value of the Property is a Collection
        /// </summary>
        public bool IsCollection { get; internal set; }

        /// <summary>
        /// Returns TRUE if the property contains a Nullable type
        /// </summary>
        public bool IsNullableType { get; internal set; }

        /// <summary>
        /// Determines if the Property can be read
        /// </summary>
        public bool CanRead
        {
            // This logic ensures that the propery is read-able, and there is a Public 'getter' method:
            get
            {
                return _AllowedOperations.CanRead &&
                        this.PropertyInfo.CanRead &&
                        (this.PropertyInfo.GetGetMethod(false) != null);
            }
        }

        /// <summary>
        /// Determines if the Property can be updated
        /// </summary>
        public bool CanWrite
        {
            // This logic ensures that the propery is write-able, and there is a Public 'setter' method:
            get
            {
                return _AllowedOperations.CanModify &&
                        this.PropertyInfo.CanWrite &&
                        (this.PropertyInfo.GetSetMethod(false) != null);
            }
        }

        /// <summary>
        /// Determines if results can be created, if this Property contains an Object or Collection
        /// </summary>
        public bool CanCreate { get; internal set; }

        /// <summary>
        /// Determines if results can be added, if this Property contains a Collection
        /// </summary>
        public bool CanAdd { get; internal set; }

        /// <summary>
        /// Determines if results can be removed, if this Property contains a Collection
        /// </summary>
        public bool CanRemove { get; internal set; }

        /// <summary>
        /// Determines if the Property value is persisted
        /// </summary>
        public bool CanPersist
        {
            get { return _Persistable.IsAllowed; }
        }

        /// <summary>
        /// Returns TRUE if the property acccepts the given type
        /// </summary>
        /// <param name="type"></param>
        public bool CanAccept(Type type)
        {
            return type.IsDerivedFrom(this.PropertyType);
        }

        /// <summary>
        /// Returns TRUE if the Property 'owns' it's contents
        /// </summary>
        public bool IsCompositeRelationship { get; internal set; }

        public bool IsAggregateRelationship { get; internal set; }

        public bool IsParentRelationship { get; internal set; }

        /// <summary>
        /// Returns the ClassTemplate which has a many-to-many relationship with the owner of this Property.
        /// Note that this function is only useful against Collection properties.
        /// </summary>

        public ClassTemplate GetManyToManyClass(TemplateCache templateCache)
        {
            if (!this.IsCollection)
                return null;

            var tCollection = (CollectionTemplate)templateCache.GetTemplate(this.PropertyInfo.PropertyType);
            var tElement = templateCache.GetTemplate(tCollection.ElementType) as ClassTemplate;
            if (tElement == null)
                return null;

            foreach (var tProperty in tElement.Properties.Values)
            {
                // We're looking for a Collection property whose InnerClass is the same as the Class containing the instance Property:
                if (!tProperty.IsCollection)
                    continue;

                var tOtherCollection = (CollectionTemplate)templateCache.GetTemplate(tProperty.PropertyInfo.PropertyType);
                var tOtherElement = templateCache.GetTemplate(tOtherCollection.ElementType) as ClassTemplate;
                if (tOtherElement == null)
                    continue;

                if (tOtherCollection.RealType.IsDerivedFrom(this.OuterClass.RealType) ||
                    this.OuterClass.RealType.IsDerivedFrom(tOtherCollection.RealType))
                {
                    // We've found the class:
                    return tElement;
                }
            }

            return null;
        }
    }
}