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

            var domainObject = new SampleModel.Objects.PocoObject();
            Assert.AreEqual(0, domainObject.ChildObjects.Count());

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(domainObject.GetType());

            // Act:
            methodCommand.Invoke(classTemplate, domainObject, "AddSomeChildObjects", null);

            // Assert:
            Assert.AreNotEqual(0, domainObject.ChildObjects);
        }


        [Test()]
        public void ShouldReturnValueFromMethod()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var domainObject = new SampleModel.MethodTests();

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(domainObject.GetType());

            // Act:
            var result = methodCommand.Invoke(classTemplate, domainObject, "MethodThatReturnsA_String", null);

            // Assert:
            Assert.IsNotNull(result);
        }

        [Test()]
        public void ShouldInvokeMethodWithArguments()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var domainObject = new SampleModel.MethodTests();

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(domainObject.GetType());

            // Act:
            var args = new object[] { DateTime.Now };
            methodCommand.Invoke(classTemplate, domainObject, "MethodWithOneParameter", args);
        }
    }
}

