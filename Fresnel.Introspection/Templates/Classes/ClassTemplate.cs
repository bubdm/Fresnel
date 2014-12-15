using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents a Object Type
    /// </summary>
    public class ClassTemplate : BaseClassTemplate
    {
        private readonly string[] _AbstractTypePrefixes = { "Abstract ", "Base ", "I " };
        private readonly string[] _AbstractTypeSuffixes = { " Base" };

        private Lazy<RapidCtor> _RapidCtor;
        private Lazy<ConstructorInfo[]> _Constructors;
        private Lazy<FieldInfoMap> _Fields;
        private Lazy<PropertyTemplateMap> _tProperties;
        private Lazy<MethodTemplateMap> _tMethods;
        private Lazy<MethodTemplateMap> _tStaticMethods;

        private ObjectInstanceAttribute _ObjectInstanceAttr;
        private Lazy<int> _InheritanceDepth;

        private DynamicMethodBuilder _DynamicMethodBuilder;
        private FieldInfoMapBuilder _FieldInfoMapBuilder;
        private PropertyTemplateMapBuilder _PropertyTemplateMapBuilder;
        private MethodTemplateMapBuilder _MethodTemplateMapBuilder;
        private StaticMethodTemplateMapBuilder _StaticMethodTemplateMapBuilder;
        private TrackingPropertiesIdentifier _TrackingPropertiesIdentifier;

        private Lazy<PropertyTemplate> _IdProperty;
        private Lazy<PropertyTemplate> _VersionProperty;
        private Lazy<PropertyTemplate> _AuditProperty;

        public ClassTemplate
        (
            DynamicMethodBuilder dynamicMethodBuilder,
            FieldInfoMapBuilder fieldInfoMapBuilder,
            PropertyTemplateMapBuilder propertyTemplateMapBuilder,
            MethodTemplateMapBuilder methodTemplateMapBuilder,
            StaticMethodTemplateMapBuilder staticMethodTemplateMapBuilder,
            TrackingPropertiesIdentifier trackingPropertiesIdentifier
        )
            : base()
        {
            _DynamicMethodBuilder = dynamicMethodBuilder;
            _FieldInfoMapBuilder = fieldInfoMapBuilder;
            _PropertyTemplateMapBuilder = propertyTemplateMapBuilder;
            _MethodTemplateMapBuilder = methodTemplateMapBuilder;
            _StaticMethodTemplateMapBuilder = staticMethodTemplateMapBuilder;
            _TrackingPropertiesIdentifier = trackingPropertiesIdentifier;

            _RapidCtor = new Lazy<RapidCtor>(
                                () => _DynamicMethodBuilder.BuildCreateObjectHandler(this.RealType),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _InheritanceDepth = new Lazy<int>(() =>
                                DetermineInheritanceDepth(this.RealType),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _Constructors = new Lazy<ConstructorInfo[]>(
                                () => this.RealType.GetConstructors(),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _Fields = new Lazy<FieldInfoMap>(
                                () => _FieldInfoMapBuilder.BuildFor(this.RealType),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _tProperties = new Lazy<PropertyTemplateMap>(
                                () => _PropertyTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _tMethods = new Lazy<MethodTemplateMap>(
                                () => _MethodTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _tStaticMethods = new Lazy<MethodTemplateMap>(
                                () => _StaticMethodTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _IdProperty = new Lazy<PropertyTemplate>(
                                () => _TrackingPropertiesIdentifier.DetermineIdProperty(this, _ObjectInstanceAttr),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _VersionProperty = new Lazy<PropertyTemplate>(
                                () => _TrackingPropertiesIdentifier.DetermineVersionProperty(this, _ObjectInstanceAttr),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _AuditProperty = new Lazy<PropertyTemplate>(
                                () => _TrackingPropertiesIdentifier.DetermineAuditProperty(this, _ObjectInstanceAttr),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _XmlComments = new Lazy<XmlComments>(
                                () => this.AssemblyReader.XmlDocReader.GetXmlCommentsFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        internal override void FinaliseConstruction()
        {
            base.FinaliseConstruction();

            this.CreateNames();

            this.DetermineInterfaces();

            _ObjectInstanceAttr = this.Attributes.Get<ObjectInstanceAttribute>();

            this.IsVisible = _ObjectInstanceAttr.IsVisible;
        }

        private void CreateNames()
        {
            // Generic objects (usually collections) need to be uniquely identifiable:
            foreach (var genericArg in this.RealType.GetGenericArguments())
            {
                this.Name += "_" + genericArg.Name;
            }

            this.FullName = string.Concat(this.RealType.Namespace, ".", this.RealType.Name);

            // Users shouldn't need to know about Abstract or Interfaces types, so let's fix that:
            if (this.RealType.IsAbstract || this.RealType.IsInterface)
            {
                this.FriendlyName = this.CreateFriendlyNameForAbstractTypes(this.FriendlyName);
            }
        }

        private string CreateFriendlyNameForAbstractTypes(string friendlyName)
        {
            var result = friendlyName;

            var stringToReplace = _AbstractTypePrefixes.SingleOrDefault(p => friendlyName.StartsWith(p));
            if (stringToReplace.IsEmpty())
            {
                stringToReplace = _AbstractTypeSuffixes.SingleOrDefault(p => friendlyName.EndsWith(p));
            }

            if (stringToReplace.IsNotEmpty())
            {
                result = string.Concat(friendlyName.Replace(stringToReplace, string.Empty), " Types");
            }

            return result;
        }

        private void DetermineInterfaces()
        {
            this.IsEntity = this.RealType.IsEntity();
            this.IsValueObject = this.RealType.IsValueObject();
            this.IsAggregateRoot = this.RealType.IsAggregateRoot();
            this.IsCloneable = this.RealType.IsDerivedFrom<ICloneable>();

            this.HasErrorInfo = this.RealType.IsDataErrorInfo();
            this.IsValidatable = this.RealType.IsValidatable();
        }

        /// <summary>
        /// Returns the Property used to retrieve the Domain Object's unique ID
        /// </summary>
        [JsonIgnore]
        public PropertyTemplate IdProperty { get { return _IdProperty.Value; } }

        /// <summary>
        /// Returns the Property used to retrieve the object's Version
        /// </summary>
        [JsonIgnore]
        public PropertyTemplate VersionProperty { get { return _VersionProperty.Value; } }

        /// <summary>
        /// Returns the Property used to retrieve the object's IAudit details
        /// </summary>
        [JsonIgnore]
        public PropertyTemplate AuditProperty { get { return _AuditProperty.Value; } }

        /// <summary>
        /// Determines if the Domain Object can be persisted to a repository
        /// </summary>
        public virtual bool IsPersistable
        {
            get
            {
                return _ObjectInstanceAttr.IsPersistable &&
                        this.IdProperty != null;
            }
        }

        /// <summary>
        /// Determins if the Domain Object has an ID property
        /// </summary>
        public bool IsTrackable
        {
            get { return this.IdProperty != null; }
        }

        /// <summary>
        /// Determines if the Domain Object implements the IEntity interface
        /// </summary>
        public bool IsEntity { get; private set; }

        /// <summary>
        /// Determines if the Domain Object implements the IValueObject interface
        /// </summary>
        public bool IsValueObject { get; private set; }

        /// <summary>
        /// Determines if the Domain Object implements the IAggregateRoot interface
        /// </summary>
        public bool IsAggregateRoot { get; private set; }

        /// <summary>
        /// Determines if the Domain Object implements the ICloneable interface
        /// </summary>
        public bool IsCloneable { get; private set; }

        /// <summary>
        /// Determines if the Domain Object has an IsValid() method
        /// </summary>
        public bool IsValidatable { get; private set; }

        /// <summary>
        /// Determines if the Domain Object implements IDataErrorInfo
        /// </summary>
        public bool HasErrorInfo { get; private set; }

        /// <summary>
        /// The collection of available public Constructors
        /// </summary>
        [JsonIgnore]
        public ConstructorInfo[] Constructors
        {
            get { return _Constructors.Value; }
        }

        /// <summary>
        /// Determines if an Object can be instantiated with zero arguments
        /// </summary>
        public bool HasDefaultConstructor
        {
            get
            {
                try
                {
                    return _RapidCtor.Value != null;
                }
                catch (Exception ex)
                {
                    // Not sure what to do here
                }
                return false;
            }
        }

        /// <summary>
        /// The collection of Fields within the Object
        /// </summary>
        [JsonIgnore]
        internal FieldInfoMap Fields
        {
            get { return _Fields.Value; }
        }

        /// <summary>
        /// The collection of PropertyTemplates associated with the Object
        /// </summary>
        public PropertyTemplateMap Properties
        {
            get { return _tProperties.Value; }
        }

        /// <summary>
        /// The collection of MethodTemplates associated with the Object
        /// </summary>
        public MethodTemplateMap Methods
        {
            get { return _tMethods.Value; }
        }

        /// <summary>
        /// The collection of MethodTemplates of the static methods in the Object Type
        /// </summary>
        public MethodTemplateMap StaticMethods
        {
            get { return _tStaticMethods.Value; }
        }

        /// <summary>
        /// Determines if an Object can be instantiated from the associated Class
        /// </summary>
        public bool IsCreatable
        {
            get
            {
                return _ObjectInstanceAttr.IsCreatable &&
                        !this.RealType.IsAbstract &&
                        this.HasDefaultConstructor;
            }
        }

        /// <summary>
        /// Returns TRUE if the class has a constructor that accepts the given argument
        /// </summary>
        /// <param name="constructorArgType"></param>

        public bool CanBeCreatedWith(Type constructorArgType)
        {
            if (constructorArgType == null)
            {
                // We're only interested in the default constructor:
                return this.HasDefaultConstructor;
            }
            else
            {
                // Find a matching ctor:
                foreach (var ctor in _Constructors.Value)
                {
                    var parameters = ctor.GetParameters();

                    if ((parameters.Length == 1) && constructorArgType.IsDerivedFrom(parameters[0].ParameterType))
                    {
                        // We've found a match:
                        return true;
                    }
                }

                return false;
            }
        }

        public object CreateInstance()
        {
            var result = _RapidCtor.Value.Invoke();
            return result;
        }

        //
        //        /// <summary>
        //        /// All of the relationships that this Object has to other Domain Objects
        //        /// </summary>
        //        public RelationshipCollection Relationships
        //        {
        //            get
        //            {
        //                if (_Relationships == null)
        //                {
        //                    _Relationships = new RelationshipCollection(this);
        //                }
        //                return _Relationships;
        //            }
        //            //internal set { _Relationships = value; }
        //        }
        //
        //        /// <summary>
        //        /// Returns TRUE if a Relationship exists with the given Target class
        //        /// </summary>
        //        /// <param name="tTargetClass"></param>
        //        
        //        /// <remarks>Humane interface</remarks>
        //        public bool HasRelationshipWith(ClassTemplate tTargetClass)
        //        {
        //            // Find a Relationship in the Target class that points back to this Property's class:
        //            var reader = tTargetClass.Relationships.Any(r => r.TargetClass == this);
        //            return reader;
        //        }
        //
        //        /// <summary>
        //        /// Returns TRUE if a Relationship exists with the given Target property
        //        /// </summary>
        //        /// <param name="tTargetProperty"></param>
        //        
        //        /// <remarks>Humane interface</remarks>
        //        public bool HasRelationshipWith(PropertyTemplate tTargetProperty)
        //        {
        //            // Find a Relationship in the Target property's class that points back to this Property:
        //            var reader = tTargetProperty.OuterClass.Relationships.Any(r => r.TargetClass == tTargetProperty.OuterClass);
        //            return reader;
        //        }

        /// <summary>
        /// Returns the depth of the Object class in an inheritance tree
        /// </summary>
        public int InheritanceDepth
        {
            get { return _InheritanceDepth.Value; }
        }

        private static int DetermineInheritanceDepth(Type objectType)
        {
            var result = 0;
            var superType = objectType.BaseType;
            while (superType != null)
            {
                result++;
                superType = superType.BaseType;
            }

            return result;
        }

    }
}
