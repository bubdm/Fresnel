using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    /// <summary>
    /// Determines which Interceptor to use when IFresnelProxy members are accessed
    /// </summary>
    public class InterceptorSelector : IInterceptorSelector
    {
        private IEnumerable<IInterceptorsSelector> _InterceptorsSelectors;

        // TODO: Refactor to make better use of AutoFac:
        public InterceptorSelector
            (
            IgnoreMethodInterceptorsSelector ignoreMethodInterceptorsSelector,
            NotifyPropertyChangedInterceptorsSelector notifyPropertyChangedInterceptorsSelector,
            NotifyCollectionChangedInterceptorsSelector notifyCollectionChangedInterceptorsSelector,
            MethodInvokeInterceptorsSelector methodInvokeInterceptorsSelector,
            CollectionAddInterceptorsSelector collectionAddInterceptorsSelector,
            CollectionRemoveInterceptorsSelector collectionRemoveInterceptorsSelector,
            PropertyGetInterceptorsSelector propertyGetnterceptorsSelector,
            PropertySetInterceptorsSelector propertySetnterceptorsSelector
            )
        {
            // NB: These are add in order of precedence:
            _InterceptorsSelectors = new List<IInterceptorsSelector>()
            {
                ignoreMethodInterceptorsSelector,
                notifyPropertyChangedInterceptorsSelector,
                notifyCollectionChangedInterceptorsSelector,
                collectionAddInterceptorsSelector,
                collectionRemoveInterceptorsSelector,
                methodInvokeInterceptorsSelector,
                propertyGetnterceptorsSelector,
                propertySetnterceptorsSelector
            
            }.ToArray();
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            //Debug.WriteLine(string.Concat("Looking for ", method.DeclaringType.Name, ".", method.Name));

            var interceptorSelector = _InterceptorsSelectors.First(s => s.CanHandle(method));

            var results = interceptorSelector.GetInterceptors(interceptors);

            return results.ToArray();
        }

    }

}
