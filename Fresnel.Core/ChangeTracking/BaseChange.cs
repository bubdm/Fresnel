using System;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public abstract class BaseChange : IDisposable
    {
        public long Sequence { get; set; }

        public virtual void Dispose()
        {
        }
    }
}