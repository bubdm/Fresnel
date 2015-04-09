using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.TestTypes;
using NUnit.Framework;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class XmlDocReadingTests
    {
        [Test]
        public void ShouldParseXmlCommentsForClass()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(TextValues);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            Assert.AreEqual("A set of Text(string) properties", tClass.XmlComments.Summary);
        }

        [Test]
        public void ShouldParseXmlCommentsForProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(TextValues);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            foreach (var tProp in tClass.Properties.Values)
            {
                Assert.IsNotEmpty(tProp.XmlComments.Summary);
            }
        }

        [Test]
        public void ShouldParseXmlCommentsForMethods()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(MethodSamples);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            foreach (var tMethod in tClass.Properties.Values)
            {
                Assert.IsNotEmpty(tMethod.XmlComments.Summary);
            }
        }

        [Test]
        public void ShouldParseXmlCommentsForMethodParameters()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var typeToInspect = typeof(MethodSamples);

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate(typeToInspect);

            // Assert:
            var tMethod = tClass.Methods["MethodWithValueParameters"];

            foreach (var tParam in tMethod.Parameters.Values)
            {
                Assert.IsNotEmpty(tParam.XmlComments.Summary);
            }
        }
    }
}