using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Envivo.Fresnel.Core.Persistence;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;

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
