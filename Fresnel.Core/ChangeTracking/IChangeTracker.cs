namespace Envivo.Fresnel.Core.ChangeTracking
{
    public interface IChangeTracker
    {
        bool IsDirty { get; set; }

        bool IsTransient { get; set; }
    }
}