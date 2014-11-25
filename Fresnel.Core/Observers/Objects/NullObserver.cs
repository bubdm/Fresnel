using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Core.Observers
{

    /// <summary>
    /// An Observer which represents a null Object. Used to implement the "Null Object" pattern
    /// </summary>
    public class NullObserver : BaseObjectObserver
    {

        public NullObserver()
            : base(null, typeof(NullObserver), new NullTemplate())
        {

        }

        public new NullTemplate Template
        {
            get { return (NullTemplate)base.Template; }
        }

    }
}
