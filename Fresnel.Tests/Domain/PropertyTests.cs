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
    public class PropertyTests
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
            pocoObject.NormalText = "1234";

            // Assert:
            var value = getCommand.Invoke(template, pocoObject, "NormalText");
            Assert.AreEqual(pocoObject.NormalText, value);
        }

        [Test()]
        public void ShouldSetProperty()
        {
            Assert.Inconclusive();
        }

    }
}

