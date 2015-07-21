using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class TestController : ApiController
    {
        private Lazy<TemplateCache> _TemplateCache;
        private Lazy<ObserverCache> _ObserverCache;

        public TestController
            (
            Lazy<TemplateCache> templateCache,
            Lazy<ObserverCache> observerCache
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
            var tClass = _TemplateCache.Value.GetTemplate(typeof(VisibilityAttribute));
            return tClass;
        }

        [HttpGet]
        public BaseObjectObserver GetObserver()
        {
            var instance = new VisibilityAttribute();
            var observer = _ObserverCache.Value.GetObserver(instance);
            return observer;
        }
    }
}