using System;
using System.Collections;
using System.Collections.Generic;


namespace Envivo.Fresnel.Introspection
{
    public interface IRealTypeResolver
    {
        Type GetRealType(Type proxyType);
    }
}