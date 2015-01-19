using System;

namespace Envivo.Fresnel.Introspection
{
    public interface IRealTypeResolver
    {
        Type GetRealType(object proxy);
    }
}