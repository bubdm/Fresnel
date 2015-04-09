using Envivo.Fresnel.Introspection;
using System;

namespace Fresnel.SampleModel.Persistence
{
    public class RealTypeResolver : IRealTypeResolver
    {
        public Type GetRealType(object proxy)
        {
            if (proxy != null)
            {
                var type = proxy.GetType();
                if (type.BaseType != null && type.Namespace == "System.Data.Entity.DynamicProxies")
                {
                    return type.BaseType;
                }
            }

            return null;
        }
    }
}