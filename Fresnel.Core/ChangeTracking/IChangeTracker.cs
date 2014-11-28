using System;
namespace Envivo.Fresnel.Core.ChangeTracking
{
    public interface IChangeTracker
    {
        bool IsDirty { get; set; }
        bool IsNewInstance { get; set; }
    }
}
