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

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ToolboxControllerTests
    {

        [Test()]
        public void ShouldReturnToolboxClassItems()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<ToolboxController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            // Act:
            var results = controller.GetClassHierarchy();

            // Assert:
            Assert.AreNotEqual(0, results.Count());
        }

        [Test()]
        public void ShouldCreateNewInstance()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<ToolboxController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            // Act:
            var result = controller.Create("Envivo.Fresnel.SampleModel.Objects.PocoObject");

            // Assert:
            Assert.IsInstanceOf<ObjectVM>(result);
        }

    }

}

