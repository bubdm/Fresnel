using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using NUnit.Framework;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ClassTemplateTests
    {
        [Test]
        public void ShouldCreateClassTemplate()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

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

            var typeToInspect = typeof(SampleModel.BasicTypes.TextValues);

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

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

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

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

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

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);

            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            // Act:
            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];

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

            var typeToInspect = typeof(SampleModel.Objects.PocoObject);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Act:
            var newInstance = createCommand.Invoke(classTemplate);

            // Assert:
            Assert.IsNotNull(newInstance);
            Assert.IsInstanceOf<SampleModel.Objects.PocoObject>(newInstance);
        }

        [Test]
        public void ShouldCreateInstanceWithArgs()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(SampleModel.Objects.DetailObject);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Act:
            var master = new SampleModel.Objects.MasterObject();
            var detail = (SampleModel.Objects.DetailObject)createCommand.Invoke(classTemplate, master);

            // Assert:
            Assert.IsNotNull(detail);
            Assert.AreSame(master, detail.Parent);
        }

        [Test]
        public void ShouldNotCreateInstanceIfCtorUnavailable()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var classTemplateBuilder = container.Resolve<ClassTemplateBuilder>();
            var attributesMapBuilder = container.Resolve<AttributesMapBuilder>();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var typeToInspect = typeof(SampleModel.StaticMethodTests);
            var attributesMap = attributesMapBuilder.BuildFor(typeToInspect);

            var classTemplate = classTemplateBuilder.BuildFor(typeToInspect, attributesMap);

            // Assert:
            Assert.Throws(typeof(IntrospectionException),
                        () => createCommand.Invoke(classTemplate));
        }

        [Test]
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