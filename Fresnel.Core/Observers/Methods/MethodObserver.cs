using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An Observer for a Method belonging to the Object
    /// </summary>

    public class MethodObserver : BaseMemberObserver
    {
        private ParameterObserverMapBuilder _ParameterObserverMapBuilder;

        private Lazy<ParameterObserverMap> _ParameterObservers;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentObject">The ObjectObserver that owns this Method</param>
        /// <param name="methodTemplate">The MethodTemplate that reflects the Method</param>
        public MethodObserver
        (
            ObjectObserver oParent,
            MethodTemplate tMethod,
            ParameterObserverMapBuilder parameterObserverMapBuilder
        )
            : base(oParent, tMethod)
        {
            _ParameterObserverMapBuilder = parameterObserverMapBuilder;

            _ParameterObservers = new Lazy<ParameterObserverMap>(
                                            () => _ParameterObserverMapBuilder.BuildFor(this),
                                            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public new MethodTemplate Template
        {
            get { return (MethodTemplate)base.Template; }
        }

        /// <summary>
        /// A set of all Parameters that belong to this Method
        /// </summary>
        public ParameterObserverMap Parameters { get { return _ParameterObservers.Value; } }

        public DateTime LastInvokedAtUtc { get; set; }
    }
}