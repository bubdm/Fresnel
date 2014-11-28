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
            ProxyMetaInterceptorsSelector proxyMetaInterceptorsSelector,
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
                proxyMetaInterceptorsSelector,
                notifyPropertyChangedInterceptorsSelector,
                notifyCollectionChangedInterceptorsSelector,
                methodInvokeInterceptorsSelector,
                collectionAddInterceptorsSelector,
                collectionRemoveInterceptorsSelector,
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

        //public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        //{
            //Debug.WriteLine(string.Concat("Looking for ", method.DeclaringType.Name, ".", method.Name));

            // Special cases:

            //if (_ObjectMemberNames.Contains(method.Name))
            //{
            //    // We don't want to intercept these methods:
            //    return results.ToArray();
            //}

            //if (method.DeclaringType.Equals(typeof(IFresnelProxy)))
            //{
            //    results.Add(this.FindInstanceOf<ProxyMetaInterceptor>(interceptors));
            //    return results.ToArray();
            //}

            //if (method.DeclaringType.Equals(typeof(INotifyPropertyChanged)))
            //{
            //    results.Add(this.FindInstanceOf<NotifyPropertyChangedInterceptor>(interceptors));
            //    return results.ToArray();
            //}

            //if (method.DeclaringType.Equals(typeof(INotifyCollectionChanged)))
            //{
            //    results.Add(this.FindInstanceOf<NotifyCollectionChangedInterceptor>(interceptors));
            //    return results.ToArray();
            //}

            // Domain Objects/Collections:

            //results.Add(this.FindInstanceOf<PrimaryInterceptor>(interceptors));

            //if (method.DeclaringType.IsCollection())
            //{
            //    if (_CollectionRemoveMethods.Contains(method.Name))
            //    {
            //        results.Add(this.FindInstanceOf<CollectionRemoveInterceptor>(interceptors));
            //        results.Add(this.FindInstanceOf<NotifyCollectionChangedInterceptor>(interceptors));
            //    }
            //    else if (_CollectionAddMethods.Contains(method.Name))
            //    {
            //        results.Add(this.FindInstanceOf<CollectionAddInterceptor>(interceptors));
            //        results.Add(this.FindInstanceOf<NotifyCollectionChangedInterceptor>(interceptors));
            //    }
            //}

            //if (method.IsSpecialName == false)
            //{
            //    results.Add(this.FindInstanceOf<MethodInvokeInterceptor>(interceptors));
            //}
            //else if (method.Name.StartsWith("get_"))
            //{
            //    results.Add(this.FindInstanceOf<PropertyGetInterceptor>(interceptors));
            //}
            //else if (method.Name.StartsWith("set_"))
            //{
            //    results.Add(this.FindInstanceOf<PropertySetInterceptor>(interceptors));
            //    results.Add(this.FindInstanceOf<NotifyPropertyChangedInterceptor>(interceptors));
            //}

            //results.Add(this.FindInstanceOf<FinalTargetInterceptor>(interceptors));

            //return results.ToArray();
        //}

        //private T FindInstanceOf<T>(IInterceptor[] interceptors)
        //    where T : class, IInterceptor
        //{
        //    for (var i = 0; i < interceptors.Length; i++)
        //    {
        //        T match = interceptors[i] as T;
        //        if (match != null)
        //        {
        //            return match;
        //        }
        //    }

        //    return default(T);
        //}

    }

}
