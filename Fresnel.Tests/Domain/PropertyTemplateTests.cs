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
    public class PropertyTemplateTests
    {

        [Test()]
        public void ShouldGetProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();

            var template = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());

            // Act:
            pocoObject.NormalText = DateTime.Now.ToString();

            // Assert:
            var value = getCommand.Invoke(template, pocoObject, "NormalText");
            Assert.AreEqual(pocoObject.NormalText, value);
        }

        [Test()]
        public void ShouldSetProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();

            var template = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());

            // Act:
            var newValue = DateTime.Now.ToString();
            setCommand.Invoke(template, pocoObject, "NormalText", newValue);
            
            // Assert:
            Assert.AreEqual(newValue, pocoObject.NormalText);
        }

        [Test()]
        public void ShouldGetBackingField()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var getCommand = container.Resolve<GetBackingFieldCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();

            var template = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());

            // Act:
            pocoObject.NormalText = DateTime.Now.ToString();

            // Assert:
            var value = getCommand.Invoke(template, pocoObject, "NormalText");
            Assert.AreEqual(pocoObject.NormalText, value);
        }

        [Test()]
        public void ShouldSetBackingField()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var setCommand = container.Resolve<SetBackingFieldCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();

            var template = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());

            // Act:
            var newValue = DateTime.Now.ToString();
            setCommand.Invoke(template, pocoObject, "NormalText", newValue);

            // Assert:
            Assert.AreEqual(newValue, pocoObject.NormalText);
        }

    }
}

