using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// Represents a collection of Parameters for a Method i.e. the methods Signature
    /// </summary>

    public class ParameterObserverMap : ReadOnlyDictionary<string, ParameterObserver>
    {
        public ParameterObserverMap(IDictionary<string, ParameterObserver> items, string signatureKey)
            : base(items)
        {
            this.SignatureKey = signatureKey;
            this.AreRequired = items.Any();
        }

        /// <summary>
        /// A value that uniquely identifies this Parameter collection
        /// </summary>
        public string SignatureKey { get; private set; }

        /// <summary>
        /// Determines if Parameter values must be provided for the associated Method
        /// </summary>
        public bool AreRequired { get; private set; }

        /// <summary>
        /// Determines if each of the Parameters has a value assigned to it
        /// </summary>
        public bool IsComplete
        {
            get { return this.Values.All(p => p.Value != null); }
        }

        /// <summary>
        /// Finds the Parameter that matches the type of the given value, and sets it to the value
        /// </summary>
        /// <param name="value"></param>
        internal void FindAndSet(object value, Type valueType)
        {
            if (value == null)
                return;

            foreach (var oParameter in this.Values)
            {
                var tParam = oParameter.Template;
                var parameterType = tParam.ParameterType;
                if (!valueType.IsDerivedFrom(parameterType))
                    continue;

                oParameter.Value = value;
                return;
            }
        }

        /// <summary>
        /// Resets the values for all of the Parameters. If the Parameter is non-reference, a default value is set:
        /// </summary>
        internal void Reset()
        {
            foreach (var oParameter in this.Values)
            {
                var tParam = oParameter.Template;
                if (tParam.IsNonReference &&
                    tParam.IsNullableType == false)
                {
                    // Set the default value:
                    oParameter.Value = Activator.CreateInstance(tParam.ParameterType);
                }
                else
                {
                    oParameter.Value = null;
                }
            }
        }
    }
}