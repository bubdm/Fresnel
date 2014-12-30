using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Proxies.ChangeTracking;
using Envivo.Fresnel.Proxies.Interceptors;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Envivo.Fresnel.Proxies
{

    public class ProxyBuilder : Envivo.Fresnel.Core.Proxies.IProxyBuilder
    {
        private ChangeLog _ChangeLog;
        private ObserverCache _ObserverCache;
        private ProxyGenerator _ProxyGenerator;
        private PropertyProxyBuilder _PropertyProxyBuilder;

        private PrimaryInterceptor _PrimaryInterceptor;
        private PropertyGetInterceptor _PropertyGetInterceptor;
        private PropertySetInterceptor _PropertySetInterceptor;
        private MethodInvokeInterceptor _MethodInvokeInterceptor;
        private CollectionAddInterceptor _CollectionAddInterceptor;
        private CollectionRemoveInterceptor _CollectionRemoveInterceptor;
        private FinalTargetInterceptor _FinalTargetInterceptor;

        private InterceptorSelector _InterceptorSelector;

        private Func<NotifyPropertyChangedInterceptor> _NotifyPropertyChangedInterceptorFactory;
        private Func<NotifyCollectionChangedInterceptor> _NotifyCollectionChangedInterceptorFactory;

        private Type[] _ObjectProxyInterfaceList;
        private Type[] _CollectionProxyInterfaceList;

        public ProxyBuilder
            (
            ObserverCache observerCache,
            ChangeLog changeLog,
            ProxyGenerator proxyGenerator,
            PropertyProxyBuilder propertyProxyBuilder,

            PrimaryInterceptor primaryInterceptor,
            PropertyGetInterceptor propertyGetInterceptor,
            PropertySetInterceptor propertySetInterceptor,
            MethodInvokeInterceptor methodInvokeInterceptor,
            CollectionAddInterceptor collectionAddInterceptor,
            CollectionRemoveInterceptor collectionRemoveInterceptor,
            FinalTargetInterceptor finalTargetInterceptor,
            InterceptorSelector interceptorSelector,

            Func<NotifyPropertyChangedInterceptor> notifyPropertyChangedInterceptorFactory,
            Func<NotifyCollectionChangedInterceptor> notifyCollectionChangedInterceptorFactory
            )
        {
            _ObserverCache = observerCache;
            _ChangeLog = changeLog;
            _ProxyGenerator = proxyGenerator;
            _PropertyProxyBuilder = propertyProxyBuilder;

            _PrimaryInterceptor = primaryInterceptor;
            _PropertyGetInterceptor = propertyGetInterceptor;
            _PropertySetInterceptor = propertySetInterceptor;
            _MethodInvokeInterceptor = methodInvokeInterceptor;
            _CollectionAddInterceptor = collectionAddInterceptor;
            _CollectionRemoveInterceptor = collectionRemoveInterceptor;
            _FinalTargetInterceptor = finalTargetInterceptor;
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

            // It's best to change the property values before we create the overall proxy:
            // This is to prevent Property interceptions kicking in unnecessarily: 
            this.InjectObjectPropertyProxies(obj, oObject);

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

            var proxyState = new ProxyState()
            {
                Meta = oObject,
                ChangeLog = _ChangeLog
            };

            var proxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = _InterceptorSelector,
            };
            proxyGenerationOptions.AddMixinInstance(proxyState);

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tClass.RealType,
                            _ObjectProxyInterfaceList,
                            obj,
                            proxyGenerationOptions,
                            CreateInterceptorsForObject()
                            );

            proxyState.ChangeLog.AddObjectCreation(oObject);

            return (IFresnelProxy)proxy;
        }

        private IInterceptor[] CreateInterceptorsForObject()
        {
            var interceptors = new IInterceptor[]
            {
                _PrimaryInterceptor,
                _PropertyGetInterceptor,
                _PropertySetInterceptor,
                _MethodInvokeInterceptor,
                // We need these interceptors to keep state for the individual Proxy:
                _NotifyPropertyChangedInterceptorFactory(),
                _FinalTargetInterceptor
            };

            return interceptors;
        }

        private IFresnelProxy CreateCollectionProxy<T>(T collection, CollectionObserver oCollection)
            where T : class
        {
            var tCollection = oCollection.Template;

            var proxyState = new ProxyState()
            {
                Meta = oCollection,
                ChangeLog = _ChangeLog
            };

            var proxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = _InterceptorSelector,
            };
            proxyGenerationOptions.AddMixinInstance(proxyState);

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tCollection.RealType,
                            _CollectionProxyInterfaceList,
                            collection,
                            proxyGenerationOptions,
                            CreateInterceptorsForCollection()
                            );

            return (IFresnelProxy)proxy;
        }

        private IInterceptor[] CreateInterceptorsForCollection()
        {
            var interceptors = new IInterceptor[]
            {
                _PrimaryInterceptor,
                _PropertyGetInterceptor,
                _PropertySetInterceptor,
                _CollectionAddInterceptor,
                _CollectionRemoveInterceptor,
                _MethodInvokeInterceptor,
                // We need these interceptors to keep state for the individual Proxy:
                _NotifyPropertyChangedInterceptorFactory(),
                _NotifyCollectionChangedInterceptorFactory(),
                _FinalTargetInterceptor
            };


            return interceptors;
        }

        private void InjectObjectPropertyProxies(object targetObject, ObjectObserver oObject)
        {
            foreach (var oProp in oObject.Properties.Values)
            {
                var tProp = oProp.Template;
                if (tProp.IsNonReference)
                    continue;

                var tCollection = tProp.InnerClass as CollectionTemplate;
                var tClass = tProp.InnerClass as ClassTemplate;

                var innerClass = tCollection != null ?
                                    tCollection.InnerClass :
                                    tClass;

                if (innerClass == null)
                    continue;

                var propertyProxy = _PropertyProxyBuilder.BuildFor(targetObject, oProp);

                tProp.SetField(targetObject, propertyProxy);
            }
        }

    }

}
