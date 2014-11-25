using System;
using Envivo.Fresnel.Introspection.Templates;

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


        ///// <summary>
        ///// Sets the parameters with the given values.  The number of items MUST match the number of parameters
        ///// </summary>
        ///// <param name="parameters"></param>
        //internal void SetParameters(params object[] parameters)
        //{
        //    if (parameters.Length == 0)
        //        return;

        //    if (parameters.Length != this.Parameters.Count)
        //        return;

        //    var i = 0;
        //    foreach (var oParameter in this.Parameters.Values)
        //    {
        //        oParameter.Value = parameters[i];
        //        i++;
        //    }
        //}

        public DateTime LastInvokedAtUtc { get; set; }

        ///// <summary>
        ///// Returns a token (Memento) for the Observer
        ///// </summary>
        //
        //
        //public MethodToken GetToken()
        //{
        //    return new MethodToken(this);
        //}

        //public override void Dispose()
        //{
        //    this.Parameters = null;
        //    base.Dispose();
        //}

    }
}
