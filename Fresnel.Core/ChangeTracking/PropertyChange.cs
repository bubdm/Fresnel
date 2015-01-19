using Envivo.Fresnel.Core.Observers;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// A snapshot of a Property at a point in time
    /// </summary>
    public class PropertyChange : BaseChange
    {
        public BasePropertyObserver Property { get; set; }

        public object OriginalValue { get; set; }

        public object NewValue { get; set; }

        public override void Dispose()
        {
            this.Property = null;
            this.OriginalValue = null;
            this.NewValue = null;
        }
    }
}