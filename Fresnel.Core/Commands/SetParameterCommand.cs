using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
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
