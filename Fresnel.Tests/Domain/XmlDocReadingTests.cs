﻿//  Author:
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
using Envivo.Fresnel.Introspection.Commands;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class XmlDocReadingTests
    {
     
        [Test()]
        public void ShouldParseXmlCommentsForClass()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            Assert.AreEqual("A set of Text(string) properties", tClass.XmlComments.Summary);
        }

        [Test()]
        public void ShouldParseXmlCommentsForProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            foreach (var tProp in tClass.Properties.Values)
            {
                Assert.IsNotEmpty(tProp.XmlComments.Summary);
            }
        }

        [Test()]
        public void ShouldParseXmlCommentsForMethods()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(SampleModel.MethodTests);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            foreach (var tMethod in tClass.Properties.Values)
            {
                Assert.IsNotEmpty(tMethod.XmlComments.Summary);
            }
        }
    }
}
