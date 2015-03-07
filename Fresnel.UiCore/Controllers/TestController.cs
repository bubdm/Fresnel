using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class TestController : ApiController
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;

        public TestController
            (
            TemplateCache templateCache,
            ObserverCache observerCache
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
        }

        [HttpGet]
        public string GetTestMessage()
        {
            return DateTime.Now.ToString();
        }

        [HttpGet]
        public IClassTemplate GetTemplate()
        {
            var tClass = _TemplateCache.GetTemplate(typeof(ObjectInstanceConfiguration));
            return tClass;
        }

        [HttpGet]
        public BaseObjectObserver GetObserver()
        {
            var instance = new ObjectInstanceConfiguration();
            var observer = _ObserverCache.GetObserver(instance);
            return observer;
        }
    }
}