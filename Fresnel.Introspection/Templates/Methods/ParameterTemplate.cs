using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// A Template that represents a Parameter in a .NET method
    /// </summary>
    /// <remarks>Technically, the Parameter isn't really a type of Member.  However, making it a part of the Member inheritance chain simplifies things.
    /// </remarks>
    public class ParameterTemplate : BaseMemberTemplate, ISettableMemberTemplate
    {
        private TemplateCache _TemplateCache;
        private Lazy<IClassTemplate> _InnerClass;

        public ParameterTemplate
        (
            TemplateCache templateCache
        )
        {
            _TemplateCache = templateCache;

            _InnerClass = new Lazy<IClassTemplate>(
                                    () => this.DetermineInnerClass(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// The .NET Reflection of the Parameter
        /// </summary>
        /// <value>A ParameterInfo object that reflects the Parameter</value>
        [JsonIgnore]
        public ParameterInfo ParameterInfo { get; internal set; }

        /// <summary>
        /// Returns the actual type of the Parameter (e.g. if it's a Nullable type)
        /// </summary>
        [JsonIgnore]
        public Type ParameterType { get; internal set; }

        /// <summary>
        /// Returns the Template for the value of the Parameter
        /// </summary>
        [JsonIgnore]
        public IClassTemplate InnerClass
        {
            get { return _InnerClass.Value; }
        }

        /// <summary>
        /// Determines if the value of the Parameter is a Non-Reference value
        /// </summary>
        /// <value>True = The value of the Parameter Is a Non-Reference value</value>
        public bool IsNonReference { get; internal set; }

        /// <summary>
        /// Determines if the value of the Parameter is an Object
        /// </summary>
        /// <value>True = The value of the Parameter is an Object</value>
        public bool IsDomainObject { get; internal set; }

        /// <summary>
        /// Determines if the value of the Parameter is a Collection
        /// </summary>
        /// <value>True = The value of the Parameter is a Collection</value>
        public bool IsCollection { get; internal set; }

        /// <summary>
        /// Determines if the value of the Parameter is a Value Object
        /// </summary>
        /// <value>True = The value of the Parameter Is a Value Object</value>
        public bool IsValueObject { get; internal set; }

        /// <summary>
        /// Returns TRUE if the property contains a Nullable type
        /// </summary>
        /// <value></value>
        public bool IsNullableType { get; internal set; }

        /// <summary>
        /// The MethodTemplate that owns this Parameter
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public MethodTemplate OuterMethod { get; internal set; }

        /// <summary>
        /// Returns TRUE if the parameter acccepts the given type
        /// </summary>
        /// <param name="type"></param>
        public bool CanAccept(Type type)
        {
            return type.IsDerivedFrom(this.ParameterType);
        }

        private IClassTemplate DetermineInnerClass()
        {
            var result = _TemplateCache.GetTemplate(this.ParameterType);
            return result;
        }
    }
}