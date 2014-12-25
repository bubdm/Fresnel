using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class CollectionRemoveInterceptorsSelector : IInterceptorsSelector
    {
        private readonly string[] _CollectionRemoveMethods;

        public CollectionRemoveInterceptorsSelector()
        {
            List<object> dummyList = null;

            _CollectionRemoveMethods = new string[]
            {
                LambdaExtensions.NameOf(() => dummyList.Remove(null)),
                LambdaExtensions.NameOf(() => dummyList.RemoveAt(0)),
            };
        }

        public bool CanHandle(MethodInfo method)
        {
            return (method.DeclaringType.IsCollection()) &&
                    _CollectionRemoveMethods.Contains(method.Name);
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            var result = new IInterceptor[]
            {
                allInterceptors.OfType<PrimaryInterceptor>().Single(),
                allInterceptors.OfType<CollectionRemoveInterceptor>().Single(),
                allInterceptors.OfType<NotifyCollectionChangedInterceptor>().Single(),
                allInterceptors.OfType<FinalTargetInterceptor>().Single(),
            };
            return result;
        }

    }

}
