using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ClassTemplateTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldCreateClassTemplate()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(TextValues);

            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Assert:
            Assert.IsNotNull(template);
        }

        [Test]
        public void ShouldGetClassTemplateFromCache()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var typeToInspect = typeof(TextValues);

            var templateCache = container.Resolve<TemplateCache>();

            // Act:
            var template = templateCache.GetTemplate(typeToInspect);

            // Assert:
            Assert.IsNotNull(template);
        }

        [Test]
        public void ShouldCreatePropertyTemplates()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(Product);

            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Assert:
            Assert.AreNotEqual(0, template.Properties.Count());

            foreach (var prop in template.Properties.Values)
            {
                Assert.IsNotNull(prop.InnerClass);
            }
        }

        [Test]
        public void ShouldCreateMethodTemplates()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(Product);

            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var template = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

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

        [Test]
        public void ShouldCreateCollectionTemplate()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(Product);

            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);
            var propName = LambdaExtensions.NameOf<Product>(x => x.Categories);
            var collectionPropertyTemplate = classTemplate.Properties[propName];

            // Assert:
            var collectionTemplate = (CollectionTemplate)collectionPropertyTemplate.InnerClass;
            Assert.IsNotNull(collectionTemplate);
            Assert.IsNotNull(collectionTemplate.InnerClass);
        }

        [Test]
        public void ShouldCreateInstanceWithNoArgs()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(Product);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Act:
            var newInstance = createCommand.Invoke(classTemplate);

            // Assert:
            Assert.IsNotNull(newInstance);
            Assert.IsInstanceOf<Product>(newInstance);
        }

        [Test]
        public void ShouldCreateInstanceWithArgs()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(OrderItem);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Act:
            var order = new Order();
            var orderItem = (OrderItem)createCommand.Invoke(classTemplate, order);

            // Assert:
            Assert.IsNotNull(orderItem);
            Assert.AreSame(order, orderItem.ParentOrder);
        }

        [Test]
        public void ShouldNotCreateInstanceIfCtorUnavailable()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(ClassWithHiddenCtor);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Assert:
            Assert.Throws<IntrospectionException>(() => createCommand.Invoke(classTemplate));
        }

        [Test]
        public void ShouldInjectDependenciesIntoCtor()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToCreate = typeof(ObjectWithCtorInjection);
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToCreate);

            var fixture = new Fixture();
            var nameToInject = fixture.Create<string>();

            // Act:
            var newInstance = (ObjectWithCtorInjection)createCommand.Invoke(tClass, nameToInject);

            // Assert:
            Assert.IsNotNull(newInstance);
            Assert.AreEqual(nameToInject, newInstance.Name);
        }
    }
}