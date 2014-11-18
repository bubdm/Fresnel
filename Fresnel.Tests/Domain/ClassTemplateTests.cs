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
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;
using System.Collections.Generic;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ClassTemplateTests
    {
        [Test()]
        public void ShouldCreateClassTemplate()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Assert:
            Assert.IsNotNull(template);
        }

        [Test()]
        public void ShouldGetClassTemplateFromCache()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

            var assemblyReaderMap = container.Resolve<AssemblyReaderMap>();
            var assemblyReader = assemblyReaderMap[typeToInspect];

            var templateCache = container.Resolve<TemplateCache>();

            // Act:
            var template = templateCache.GetTemplate(typeToInspect);

            // Assert:
            Assert.IsNotNull(template);
        }


        [Test()]
        public void ShouldCreatePropertyTemplates()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Assert:
            Assert.AreNotEqual(0, template.Properties.Count());
        }

        [Test()]
        public void ShouldCreateMethodTemplates()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Assert:
            Assert.AreNotEqual(0, template.Methods.Count());

            foreach (var method in template.Methods.Values)
            {
                var parameters = method.Parameters;
                Assert.IsNotNull(method.Parameters);
            }
        }

    }
}

