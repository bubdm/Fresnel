using Envivo.Fresnel.Core.Observers;

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