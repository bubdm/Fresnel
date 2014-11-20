using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Engine.Commands
{
    public class SetParameterCommand
    {

        public void Invoke(ParameterObserver oParameter, BaseObjectObserver oParameterValue)
        {
            oParameter.InnerObserver = oParameterValue;
            oParameter.Value = oParameterValue.RealObject;
        }

    }
}
