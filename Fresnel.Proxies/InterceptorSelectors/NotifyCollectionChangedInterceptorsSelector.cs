using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class NotifyCollectionChangedInterceptorsSelector : IInterceptorsSelector
    {
        
        public bool CanHandle(MethodInfo method)
        {
            return method.DeclaringType.Equals(typeof(INotifyCollectionChanged));
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            return allInterceptors
                    .Where(i => i is NotifyCollectionChangedInterceptor)
                    .ToArray();
        }

    }

}
