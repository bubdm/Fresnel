using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// A Template that represents a method in a .NET class
    /// </summary>

    public class MethodTemplate : BaseMemberTemplate
    {
        private DynamicMethodBuilder _DynamicMethodBuilder;
        private ParameterTemplateMapBuilder _ParameterTemplateMapBuilder;
        private Lazy<ParameterTemplateMap> _Parameters;
        private Lazy<RapidMethod> _RapidMethod;

        private MethodConfiguration _Attribute;

        public MethodTemplate
        (
            DynamicMethodBuilder dynamicMethodBuilder,
            ParameterTemplateMapBuilder parameterTemplateMapBuilder
        )
        {
            _DynamicMethodBuilder = dynamicMethodBuilder;
            _ParameterTemplateMapBuilder = parameterTemplateMapBuilder;

            _RapidMethod = new Lazy<RapidMethod>(
                                () => _DynamicMethodBuilder.BuildMethodHandler(this.MethodInfo),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _Parameters = new Lazy<ParameterTemplateMap>(
                                () => _ParameterTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _XmlComments = new Lazy<XmlComments>(
                                () => this.AssemblyReader.XmlDocReader.GetXmlCommentsFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        internal override void FinaliseConstruction()
        {
            _Attribute = this.Configurations.Get<MethodConfiguration>();

            base.FinaliseConstruction();
        }

        /// <summary>
        /// Executes the method on the given Object with the given parameters
        /// </summary>
        /// <param name="obj">The instance to execute the method on</param>
        /// <param name="args"></param>

        public object Invoke(object obj, IEnumerable<object> args)
        {
            var rapidMethod = _RapidMethod.Value;

            var argsArray = args != null ?
                            args.ToArray() :
                            null;

            if (rapidMethod != null)
            {
                return rapidMethod.Invoke(obj, argsArray);
            }
            else
            {
                // Fallback is using standard Reflection:
                return this.MethodInfo.Invoke(obj, argsArray);
            }
        }

        ///// <summary>
        ///// Determines if the method has a paramter that is a Service
        ///// </summary>
        //public bool UsesDoubleDispatch { get; private set; }

        /// <summary>
        /// The .NET Reflection of the Method
        /// </summary>
        /// <value>A MethodInfo object that reflects the Method</value>
        [JsonIgnore]
        public MethodInfo MethodInfo { get; internal set; }

        /// <summary>
        /// The collection of ParameterTemplates associated with the Method
        /// </summary>
        /// <value></value>

        public ParameterTemplateMap Parameters
        {
            get { return _Parameters.Value; }
        }

        ///// <summary>
        ///// Determines if the Method can be invoked
        ///// </summary>
        ///// <value></value>
        //
        //
        //public bool CanInvoke
        //{
        //    get { return _Attribute.IsVisible; }
        //}

        /// <summary>
        /// Returns TRUE if this Method is used for adding results to Collections
        /// </summary>
        public bool IsCollectionAddMethod { get; internal set; }

        /// <summary>
        /// Returns TRUE if this Method is used for removing results from Collections
        /// </summary>
        public bool IsCollectionRemoveMethod { get; internal set; }

        ///// <summary>
        ///// Appends the given value to the Name and FullName
        ///// </summary>
        ///// <param name="extension"></param>
        //
        internal void AppendNameWith(string extension)
        {
            if (extension.IsEmpty())
                return;

            this.Name += extension;
            this.FullName += extension;
        }
    }
}