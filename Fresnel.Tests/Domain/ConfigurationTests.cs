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
            var tMethod1 = tClass.Methods["NormalBoolean"];
            var tMethod2 = tClass.Methods["Orientation"];

            // Assert:
            Assert.AreEqual("Yes", tMethod1.Configurations.Get<BooleanTrueValueAttribute>().Name);
            Assert.AreEqual("No", tMethod1.Configurations.Get<BooleanFalseValueAttribute>().Name);

            Assert.AreEqual("Clockwise", tMethod2.Configurations.Get<BooleanTrueValueAttribute>().Name);
            Assert.AreEqual("Anti-Clockwise", tMethod2.Configurations.Get<BooleanFalseValueAttribute>().Name);

        }

    }
}