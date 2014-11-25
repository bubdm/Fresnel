using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Envivo.Fresnel.Proxies
{

    public class ProxyBuilder : Envivo.Fresnel.Core.Proxies.IProxyBuilder
    {
        private ObserverCache _ObserverCache;

        private PrimaryInterceptor _PrimaryInterceptor;
        private PropertyGetInterceptor _PropertyGetInterceptor;
        private PropertySetInterceptor _PropertySetInterceptor;
        private MethodInvokeInterceptor _MethodInvokeInterceptor;
        private CollectionAddInterceptor _CollectionAddInterceptor;
        private CollectionRemoveInterceptor _CollectionRemoveInterceptor;
        private FinalTargetInterceptor _FinalTargetInterceptor;

        private ProxyGenerator _ProxyGenerator = new ProxyGenerator();
        private ProxyGenerationOptions _ProxyGenerationOptions;

        private Type[] _ObjectProxyInterfaceList;
        private Type[] _CollectionProxyInterfaceList;

        public ProxyBuilder
            (
            ObserverCache observerCache,
            PrimaryInterceptor primaryInterceptor,
            PropertyGetInterceptor propertyGetInterceptor,
            PropertySetInterceptor propertySetInterceptor,
            MethodInvokeInterceptor methodInvokeInterceptor,
            CollectionAddInterceptor collectionAddInterceptor,
            CollectionRemoveInterceptor collectionRemoveInterceptor,
            FinalTargetInterceptor finalTargetInterceptor,

            InterceptorSelector interceptorSelector
            )
        {
            _ObserverCache = observerCache;

            _PrimaryInterceptor = primaryInterceptor;
            _PropertyGetInterceptor = propertyGetInterceptor;
            _PropertySetInterceptor = propertySetInterceptor;
            _MethodInvokeInterceptor = methodInvokeInterceptor;
            _CollectionAddInterceptor = collectionAddInterceptor;
            _CollectionRemoveInterceptor = collectionRemoveInterceptor;
            _FinalTargetInterceptor = finalTargetInterceptor;

            this.InitialseProxyInterfaceLists();

            _ProxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = interceptorSelector,
            };
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


        public IFresnelProxy BuildFor(object obj)
        {
            //Debug.WriteLine(string.Concat("Creating ViewModel for ", oObj.DebugID));

            var observer = _ObserverCache.GetObserver(obj);

            var oObject = observer as ObjectObserver;
            var oCollection = observer as CollectionObserver;

            var result = oCollection != null ?
                            this.CreateCollectionProxy(obj, oCollection) :
                            this.CreateObjectProxy(obj, oObject);

            if (result.Equals(obj) == false)
            {
                var msg = string.Concat("Generated proxy is not equivalent to original object. ",
                                        "Check that ", observer.Template.Name, ".Equals() and ",
                                        observer.Template.Name, ".GetHashCode() are overridden correctly.");
                throw new FresnelException(msg);
            }

            return result;
        }

        private IFresnelProxy CreateObjectProxy<T>(T obj, ObjectObserver oObject)
            where T : class
        {
            var tClass = oObject.TemplateAs<ClassTemplate>();
         
            // We need these interceptors to keep state for the individual Proxy:
            var metaInterceptor = new ProxyMetaInterceptor(oObject);
            var notifyPropertyChangedInterceptor = new NotifyPropertyChangedInterceptor();

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tClass.RealObjectType,
                            _ObjectProxyInterfaceList,
                            obj,
                            _ProxyGenerationOptions,
                            metaInterceptor,
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
            var tCollection = oCollection.TemplateAs<CollectionTemplate>();

            // We need these interceptors to keep state for the individual Proxy:
            var metaInterceptor = new ProxyMetaInterceptor(oCollection);
            var notifyPropertyChangedInterceptor = new NotifyPropertyChangedInterceptor();
            var notifyCollectionChangedInterceptor = new NotifyCollectionChangedInterceptor();

            var proxy = _ProxyGenerator
                            .CreateClassProxyWithTarget(
                            tCollection.RealObjectType,
                            _CollectionProxyInterfaceList,
                            collection,
                            _ProxyGenerationOptions,
                            metaInterceptor,
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
