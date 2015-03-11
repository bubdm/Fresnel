using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ConfigurationTests
    {
        [Test]
        public void ShouldBuildConfigurationForBooleanProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<SampleModel.BasicTypes.BooleanValues>();

            // Act:
            var tMethod1 = tClass.Properties["NormalBoolean"];
            var tMethod2 = tClass.Properties["Orientation"];

            // Assert:
            var booleanAttrForMethod1 = tMethod1.Attributes.Get<DisplayBooleanAttribute>();
            Assert.AreEqual("Yes", booleanAttrForMethod1.TrueValue);
            Assert.AreEqual("No", booleanAttrForMethod1.FalseValue);

            var booleanAttrForMethod2 = tMethod2.Attributes.Get<DisplayBooleanAttribute>();
            Assert.AreEqual("Clockwise", booleanAttrForMethod2.TrueValue);
            Assert.AreEqual("Anti-Clockwise", booleanAttrForMethod2.FalseValue);

        }

    }
}