using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Proxies.ChangeTracking;
using Envivo.Fresnel.Proxies.Interceptors;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Envivo.Fresnel.Proxies
{

    public class ProxyBuilder : Envivo.Fresnel.Core.Proxies.IProxyBuilder
    {
        private SessionJournal _SessionJournal;
        private ObserverCache _ObserverCache;

        private PrimaryInterceptor _PrimaryInterceptor;
        private PropertyGetInterceptor _PropertyGetInterceptor;
        private PropertySetInterceptor _PropertySetInterceptor;
        private MethodInvokeInterceptor _MethodInvokeInterceptor;
        private CollectionAddInterceptor _CollectionAddInterceptor;
        private CollectionRemoveInterceptor _CollectionRemoveInterceptor;
        private ProxyMetaInterceptor _ProxyMetaInterceptor;
        private FinalTargetInterceptor _FinalTargetInterceptor;

        private InterceptorSelector _InterceptorSelector;

        private Func<NotifyPropertyChangedInterceptor> _NotifyPropertyChangedInterceptorFactory;
        private Func<NotifyCollectionChangedInterceptor> _NotifyCollectionChangedInterceptorFactory;

        private ProxyGenerator _ProxyGenerator = new ProxyGenerator();

        private Type[] _ObjectProxyInterfaceList;
        private Type[] _CollectionProxyInterfaceList;

        public ProxyBuilder
            (
            ObserverCache observerCache,
            SessionJournal sessionJournal,
            PrimaryInterceptor primaryInterceptor,
            PropertyGetInterceptor propertyGetInterceptor,
            PropertySetInterceptor propertySetInterceptor,
            MethodInvokeInterceptor methodInvokeInterceptor,
            CollectionAddInterceptor collectionAddInterceptor,
            CollectionRemoveInterceptor collectionRemoveInterceptor,
            FinalTargetInterceptor finalTargetInterceptor,
            ProxyMetaInterceptor proxyMetaInterceptor,
            InterceptorSelector interceptorSelector,

            Func<NotifyPropertyChangedInterceptor> notifyPropertyChangedInterceptorFactory,
            Func<NotifyCollectionChangedInterceptor> notifyCollectionChangedInterceptorFactory
            )
        {
            _ObserverCache = observerCache;
            _SessionJournal = sessionJournal;

            _PrimaryInterceptor = primaryInterceptor;
            _PropertyGetInterceptor = propertyGetInterceptor;
            _PropertySetInterceptor = propertySetInterceptor;
            _MethodInvokeInterceptor = methodInvokeInterceptor;
            _CollectionAddInterceptor = collectionAddInterceptor;
            _CollectionRemoveInterceptor = collectionRemoveInterceptor;
            _FinalTargetInterceptor = finalTargetInterceptor;
            _ProxyMetaInterceptor = proxyMetaInterceptor;
            _InterceptorSelector = interceptorSelector;

            _NotifyPropertyChangedInterceptorFactory = notifyPropertyChangedInterceptorFactory;
            _NotifyCollectionChangedInterceptorFactory = notifyCollectionChangedInterceptorFactory;

            this.InitialseProxyInterfaceLists();
        }

        private void InitialseProxyInterfaceLists()
        {
            _ObjectProxyInterfaceList = new Type[] 
            {
                typeof(INotifyPropertyChanged),
                typeof(IFresnelProxy) 
            };

            _CollectionProxyInterfaceList = new Type[] 
            {
                typeof(INotifyPropertyChanged),
                typeof(INotifyCollectionChanged),
                typeof(IFresnelProxy) 
            };
        }


        public IFresnelProxy BuildFor(object obj, BaseObjectObserver observer)
        {
            var oObject = observer as ObjectObserver;
            var oCollection = observer as CollectionObserver;

            var result = oCollection != null ?
                            this.CreateCollectionProxy(obj, oCollection) :
                            this.CreateObjectProxy(obj, oObject);

#if DEBUG
            if (!result.GetType().IsDerivedFrom(observer.Template.RealType))
            {
                var msg = string.Concat("Generated proxy is not equivalent to original object. ",
                                        "Check that ", observer.Template.Name, ".Equals() and ",
                                        observer.Template.Name, ".GetHashCode() are overridden correctly.");
                throw new FresnelException(msg);
            }
#endif

            return result;
        }

        private IFresnelProxy CreateObjectProxy<T>(T obj, ObjectObserver oObject)
            where T : class
        {
            var tClass = oObject.Template;

            // We need these interceptors to keep state for the individual Proxy:
            var notifyPropertyChangedInterceptor = _NotifyPropertyChangedInterceptorFactory();

            var proxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = _InterceptorSelector,
            };
            var proxyState = new ProxyState()
            {
                Meta = oObject,
                SessionJournal = _SessionJournal
            };
            proxyGenerationOptions.AddMixinInstance(proxyState);

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tClass.RealType,
                            _ObjectProxyInterfaceList,
                            obj,
                            proxyGenerationOptions,
                            _ProxyMetaInterceptor,
                            _PrimaryInterceptor,
                            _PropertyGetInterceptor,
                            _PropertySetInterceptor,
                            _MethodInvokeInterceptor,
                            notifyPropertyChangedInterceptor,
                            _FinalTargetInterceptor
                            );

            return (IFresnelProxy)proxy;
        }

        private IFresnelProxy CreateCollectionProxy<T>(T collection, CollectionObserver oCollection)
            where T : class
        {
            var tCollection = oCollection.Template;

            // We need these interceptors to keep state for the individual Proxy:
            var notifyPropertyChangedInterceptor = _NotifyPropertyChangedInterceptorFactory();
            var notifyCollectionChangedInterceptor = _NotifyCollectionChangedInterceptorFactory();

            var proxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = _InterceptorSelector,
            };
            var proxyState = new ProxyState()
            {
                Meta = oCollection,
                SessionJournal = _SessionJournal
            };
            proxyGenerationOptions.AddMixinInstance(proxyState);

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tCollection.RealType,
                            _CollectionProxyInterfaceList,
                            collection,
                            proxyGenerationOptions,
                            _ProxyMetaInterceptor,
                            _PrimaryInterceptor,
                            _PropertyGetInterceptor,
                            _PropertySetInterceptor,
                            _CollectionAddInterceptor,
                            _CollectionRemoveInterceptor,
                            _MethodInvokeInterceptor,
                            notifyPropertyChangedInterceptor,
                            notifyCollectionChangedInterceptor,
                            _FinalTargetInterceptor
                            );

            return (IFresnelProxy)proxy;
        }

    }

}
