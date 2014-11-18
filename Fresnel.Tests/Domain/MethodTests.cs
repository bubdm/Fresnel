//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using Autofac;
using NUnit.Framework;
using System;
using System.Linq;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class MethodTests
    {

        [Test()]
        public void ShouldInvokeMethod()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();
            Assert.AreEqual(0, pocoObject.ChildObjects.Count());

            var template = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());

            // Act:
            methodCommand.Invoke(template, pocoObject, "AddSomeChildObjects", null);

            // Assert:
            Assert.AreNotEqual(0, pocoObject.ChildObjects);
        }

    }
}

