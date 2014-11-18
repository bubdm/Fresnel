using System;
using System.Collections;
using System.Collections.Generic;


namespace Envivo.Fresnel.Introspection
{
    public interface ITypeResolverStrategy
    {
        Type GetRealType(Type proxiedType);
    }
}