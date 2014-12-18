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
using Envivo.Fresnel.Introspection.Commands;

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

            foreach (var prop in template.Properties.Values)
            {
                Assert.IsNotNull(prop.InnerClass);
            }
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

                foreach (var p in parameters.Values)
                {
                    Assert.IsNotNull(p.InnerClass);
                }
            }
        }

        [Test()]
        public void ShouldCreateCollectionTemplate()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributes);
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];

            // Assert:
            var collectionTemplate = (CollectionTemplate)collectionPropertyTemplate.InnerClass;
            Assert.IsNotNull(collectionTemplate);
            Assert.IsNotNull(collectionTemplate.InnerClass);
        }

        [Test()]
        public void ShouldCreateInstanceWithNoArgs()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);
            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Act:
            var newInstance = createCommand.Invoke(classTemplate);

            // Assert:
            Assert.IsNotNull(newInstance);
            Assert.IsInstanceOf<SampleModel.Objects.PocoObject>(newInstance);
        }

        [Test()]
        public void ShouldCreateInstanceWithArgs()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(SampleModel.Objects.DetailObject);
            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Act:
            var master = new SampleModel.Objects.MasterObject();
            var detail = (SampleModel.Objects.DetailObject)createCommand.Invoke(classTemplate, master);

            // Assert:
            Assert.IsNotNull(detail);
            Assert.AreSame(master, detail.Parent);
        }

        [Test()]
        public void ShouldNotCreateInstanceIfCtorUnavailable()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(SampleModel.StaticMethodTests);
            var attributes = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributes);

            // Assert:
            Assert.Throws(typeof(FresnelException),
                        () => createCommand.Invoke(classTemplate));
        }

        [Test()]
        public void ShouldInjectDependenciesIntoCtor()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToCreate = typeof(SampleModel.Objects.DependencyAwareObject);
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToCreate);

            // Act:
            var newInstance = (SampleModel.Objects.DependencyAwareObject)createCommand.Invoke(tClass, "test");

            // Assert:
            Assert.IsNotNull(newInstance);
            Assert.IsNotNull(newInstance.PocoObject);
            Assert.AreEqual("test", newInstance.Name);
        }

    }
}

