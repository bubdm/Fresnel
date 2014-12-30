//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
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
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Objects;
using System.Threading;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class SessionControllerTests
    {

        [Test()]
        public void ShouldReturnSingleSession()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<SessionController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var now = DateTime.Now;

            // Act:
            var session1 = controller.GetSession();
            Thread.Sleep(100);

            var session2 = controller.GetSession();

            // Assert:
            Assert.AreEqual(Environment.UserName, session1.UserName);
            Assert.GreaterOrEqual(session1.LogonTime, now);
            Assert.AreEqual(session1.LogonTime, session2.LogonTime);
        }

    }

}

