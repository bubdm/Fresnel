using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Proxies;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Proxies;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class TestController : ApiController
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private ProxyCache _ProxyCache;

        public TestController
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            ProxyCache proxyCache
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ProxyCache = proxyCache;
        }

        [HttpGet]
        public string GetTestMessage()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet]
        public IClassTemplate GetTemplate()
        {
            var tClass = _TemplateCache.GetTemplate(typeof(ObjectInstanceAttribute));
            return tClass;
        }

        [HttpGet]
        public BaseObjectObserver GetObserver()
        {
            var instance = new ObjectInstanceAttribute();
            var observer = _ObserverCache.GetObserver(instance);
            return observer;
        }

        [HttpGet]
        public IFresnelProxy GetProxy()
        {
            var instance = new ObjectInstanceAttribute();
            var proxy = (IFresnelProxy)_ProxyCache.GetProxy(instance);
            return proxy;
        }

    }
}