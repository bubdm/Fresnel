using Envivo.Fresnel.Core.Observers;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public class ObjectCreation : BaseChange
    {
        public ObjectObserver Object { get; set; }

        public override void Dispose()
        {
            this.Object = null;
        }
    }
}