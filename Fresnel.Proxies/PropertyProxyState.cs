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

    internal class PropertyProxyState : IPropertyProxy
    {
        public PropertyTemplate PropertyTemplate { get; set; }

        public object OuterObject { get; set; }

        public object OriginalPropertyValue { get; set; }

    }

}
