using System;

namespace Envivo.Fresnel.Utils
{
    public static class ObjectExtensions
    {
        public static void DisposeSafely(this IDisposable obj)
        {
            if (obj == null)
                return;

            obj.Dispose();
        }
    }
}