using System;
namespace Envivo.Fresnel.Core.ChangeTracking
{
    public interface IChangeTracker
    {
        bool HasChanges { get; set; }
        bool IsDirty { get; }
        bool IsNewInstance { get; set; }
    }
}
