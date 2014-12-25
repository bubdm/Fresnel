using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class CollectionAddInterceptorsSelector : IInterceptorsSelector
    {
        private readonly string[] _CollectionAddMethods;

        public CollectionAddInterceptorsSelector()
        {
            List<object> dummyList = null;

            _CollectionAddMethods = new string[]
            {
                LambdaExtensions.NameOf(() => dummyList.Add(null)),
                LambdaExtensions.NameOf(() => dummyList.Insert(0, null)),
            };
        }

        public bool CanHandle(MethodInfo method)
        {
            return (method.DeclaringType.IsCollection()) &&
                    _CollectionAddMethods.Contains(method.Name);
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            var result = new IInterceptor[]
            {
                allInterceptors.OfType<PrimaryInterceptor>().Single(),
                allInterceptors.OfType<CollectionAddInterceptor>().Single(),
                allInterceptors.OfType<NotifyCollectionChangedInterceptor>().Single(),
                allInterceptors.OfType<FinalTargetInterceptor>().Single(),
            };
            return result;
        }

    }

}
