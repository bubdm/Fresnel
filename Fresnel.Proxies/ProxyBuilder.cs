using Castle.DynamicProxy;
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
        private TemplateCache _TemplateCache;

        private PrimaryInterceptor _PrimaryInterceptor;
        private PropertyGetInterceptor _PropertyGetInterceptor;
        private PropertySetInterceptor _PropertySetInterceptor;
        private MethodInvokeInterceptor _MethodInvokeInterceptor;
        private CollectionAddInterceptor _CollectionAddInterceptor;
        private CollectionRemoveInterceptor _CollectionRemoveInterceptor;
        private NotifyPropertyChangedInterceptor _NotifyPropertyChangedInterceptor;
        private NotifyCollectionChangedInterceptor _NotifyCollectionChangedInterceptor;

        private ProxyGenerator _ProxyGenerator = new ProxyGenerator();
        private ProxyGenerationOptions _ProxyGenerationOptions;

        private Type[] _ObjectProxyInterfaceList;
        private Type[] _CollectionProxyInterfaceList;

        public ProxyBuilder
            (
            TemplateCache templateCache,
            PrimaryInterceptor primaryInterceptor,
            PropertyGetInterceptor propertyGetInterceptor,
            PropertySetInterceptor propertySetInterceptor,
            MethodInvokeInterceptor methodInvokeInterceptor,
            CollectionAddInterceptor collectionAddInterceptor,
            CollectionRemoveInterceptor collectionRemoveInterceptor,
            NotifyPropertyChangedInterceptor notifyPropertyChangedInterceptor,
            NotifyCollectionChangedInterceptor notifyCollectionChangedInterceptor
            )
        {
            _TemplateCache = templateCache;

            _PrimaryInterceptor = primaryInterceptor;
            _PropertyGetInterceptor = propertyGetInterceptor;
            _PropertySetInterceptor = propertySetInterceptor;
            _MethodInvokeInterceptor = methodInvokeInterceptor;
            _CollectionAddInterceptor = collectionAddInterceptor;
            _CollectionRemoveInterceptor = collectionRemoveInterceptor;
            _NotifyPropertyChangedInterceptor = notifyPropertyChangedInterceptor;
            _NotifyCollectionChangedInterceptor = notifyCollectionChangedInterceptor;

            this.InitialseProxyInterfaceLists();

            _ProxyGenerationOptions = new ProxyGenerationOptions()
            {
                Selector = new InterceptorSelector()
            };
        }

        private void InitialseProxyInterfaceLists()
        {
            _ObjectProxyInterfaceList = new Type[] {typeof(INotifyPropertyChanged),
                                                    typeof(IFresnelProxy) };

            _CollectionProxyInterfaceList = new Type[] {typeof(INotifyPropertyChanged),
                                                        typeof(INotifyCollectionChanged),
                                                        typeof(IFresnelProxy) };
        }


        public IFresnelProxy BuildFor<T>(T obj)
            where T : class
        {
            //Debug.WriteLine(string.Concat("Creating ViewModel for ", oObj.DebugID));

            var tClass = _TemplateCache.GetTemplate<T>() as ClassTemplate;
            var tCollection = _TemplateCache.GetTemplate<T>() as CollectionTemplate;

            var result = tCollection != null ?
                            this.CreateCollectionProxy(obj, tCollection) :
                            this.CreateObjectProxy(obj, tClass);

            if (result.Equals(obj) == false)
            {
                var msg = string.Concat("Generated proxy is not equivalent to original object. ",
                                        "Check that ", tClass.Name, ".Equals() and ", tClass.Name, ".GetHashCode() are overridden correctly.");
                throw new FresnelException(msg);
            }

            return result;
        }

        private IFresnelProxy CreateObjectProxy<T>(T obj, ClassTemplate tClass)
            where T : class
        {
            return (IFresnelProxy)_ProxyGenerator.CreateClassProxyWithTarget(
                                                     tClass.RealObjectType,
                                                     _ObjectProxyInterfaceList,
                                                     obj,
                                                     _ProxyGenerationOptions,
                                                     _PrimaryInterceptor,
                                                     _PropertyGetInterceptor,
                                                     _PropertySetInterceptor,
                                                     _MethodInvokeInterceptor,
                                                     _NotifyPropertyChangedInterceptor);
        }

        private IFresnelProxy CreateCollectionProxy<T>(T collection, CollectionTemplate tCollection)
            where T : class
        {
            return (IFresnelProxy)_ProxyGenerator.CreateClassProxyWithTarget(
                                                    tCollection.RealObjectType,
                                                    _CollectionProxyInterfaceList,
                                                    collection,
                                                    _ProxyGenerationOptions,
                                                    _PrimaryInterceptor,
                                                    _PropertyGetInterceptor,
                                                    _PropertySetInterceptor,
                                                    _CollectionAddInterceptor,
                                                    _CollectionRemoveInterceptor,
                                                    _MethodInvokeInterceptor,
                                                    _NotifyPropertyChangedInterceptor,
                                                    _NotifyCollectionChangedInterceptor);
        }

    }

}
