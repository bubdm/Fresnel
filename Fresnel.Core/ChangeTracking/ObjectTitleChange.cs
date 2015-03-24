using Envivo.Fresnel.Core.Observers;

namespace Envivo.Fresnel.Core.ChangeTracking
{

    public class ObjectTitleChange : BaseChange
    {
        public BaseObjectObserver ObjectObserver { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public override void Dispose()
        {
            this.OriginalValue = null;
            this.NewValue = null;
        }
    }
}