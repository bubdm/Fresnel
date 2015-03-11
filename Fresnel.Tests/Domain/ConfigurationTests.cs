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
            var tNormalBool = tClass.Properties["NormalBoolean"];
            var tOrientation = tClass.Properties["Orientation"];

            // Assert:
            var booleanAttr1 = tNormalBool.Attributes.Get<DisplayBooleanAttribute>();
            Assert.AreEqual("Yes", booleanAttr1.TrueValue);
            Assert.AreEqual("No", booleanAttr1.FalseValue);

            var booleanAttr2 = tOrientation.Attributes.Get<DisplayBooleanAttribute>();
            Assert.AreEqual("Clockwise", booleanAttr2.TrueValue);
            Assert.AreEqual("Anti-Clockwise", booleanAttr2.FalseValue);
        }

        [Test]
        public void ShouldBuildConfigurationForStringProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<SampleModel.BasicTypes.TextValues>();

            // Act:
            var tReadOnlyText = tClass.Properties["ReadOnlyText"];
            var tHiddenText = tClass.Properties["HiddenText"];
            var tMultiLine = tClass.Properties["MultiLineText"];
            var tPassword = tClass.Properties["PasswordText"];
            var tTextWithSize = tClass.Properties["TextWithSize"];

            // Assert:
            Assert.AreEqual(true, tReadOnlyText.Attributes.Get<VisibilityAttribute>().IsAllowed);
            Assert.AreEqual(false, tHiddenText.Attributes.Get<VisibilityAttribute>().IsAllowed);

            Assert.AreEqual(DataType.Custom, tReadOnlyText.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.MultilineText, tMultiLine.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.Password, tPassword.Attributes.Get<DataTypeAttribute>().DataType);

            Assert.AreEqual(8, tTextWithSize.Attributes.Get<MinLengthAttribute>().Length);
            Assert.AreEqual(16, tTextWithSize.Attributes.Get<MaxLengthAttribute>().Length);
        }


        [Test]
        public void ShouldBuildConfigurationForDateTimeProperty()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldBuildConfigurationForNumberProperty()
        {
            throw new NotImplementedException();
        }

    }
}