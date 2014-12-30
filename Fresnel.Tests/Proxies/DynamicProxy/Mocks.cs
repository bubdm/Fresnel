using NUnit.Framework;
using Autofac;
using System;
using System.Linq;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;
using System.Collections.Generic;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Proxies;
using System.ComponentModel;
using Envivo.Fresnel.DomainTypes;
using System.Diagnostics;

namespace Envivo.Fresnel.Tests.Proxies.DynamicProxy
{
    public interface IProxy
    {

    }

    public class A
    {
        private string _Value = "Some Value";

        /// <summary>
        /// This should return the value from the private field
        /// </summary>
        /// <returns></returns>
        public virtual string GetValue()
        {
            return _Value;
        }

        /// <summary>
        /// This calls GetValue()
        /// </summary>
        /// <returns></returns>
        public virtual string GetValue2()
        {
            return this.GetValue();
        }

        /// <summary>
        /// This calls calls GetValue2(), which calls GetValue()
        /// </summary>
        public virtual string Value
        {
            get { return this.GetValue2(); }
            set { _Value = value; }
        }

    }

    public class B : A
    {

    }


}

