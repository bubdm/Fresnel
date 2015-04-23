using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.Utils;
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

            var tClass = (ClassTemplate)templateCache.GetTemplate<BooleanValues>();

            // Act:
            var tNormalBool = tClass.Properties[LambdaExtensions.NameOf<BooleanValues>(x=> x.NormalBoolean)];
            var tOrientation = tClass.Properties[LambdaExtensions.NameOf<BooleanValues>(x => x.Orientation)];

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

            var tClass = (ClassTemplate)templateCache.GetTemplate<TextValues>();

            // Act:
            var tReadOnlyText = tClass.Properties[LambdaExtensions.NameOf<TextValues>(x=> x.ReadOnlyText)];
            var tHiddenText = tClass.Properties[LambdaExtensions.NameOf<TextValues>(x => x.HiddenText)];
            var tMultiLine = tClass.Properties[LambdaExtensions.NameOf<TextValues>(x => x.MultiLineText)];
            var tPassword = tClass.Properties[LambdaExtensions.NameOf<TextValues>(x => x.PasswordText)];
            var tTextWithSize = tClass.Properties[LambdaExtensions.NameOf<TextValues>(x => x.TextWithSize)];

            // Assert:
            Assert.IsTrue(tReadOnlyText.Attributes.Get<VisibilityAttribute>().IsAllowed);
            Assert.IsFalse(tHiddenText.Attributes.Get<VisibilityAttribute>().IsAllowed);

            Assert.AreEqual(DataType.Text, tReadOnlyText.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.MultilineText, tMultiLine.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.Password, tPassword.Attributes.Get<DataTypeAttribute>().DataType);

            Assert.AreEqual(8, tTextWithSize.Attributes.Get<MinLengthAttribute>().Length);
            Assert.AreEqual(16, tTextWithSize.Attributes.Get<MaxLengthAttribute>().Length);
        }

        [Test]
        public void ShouldBuildConfigurationForDateTimeProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<DateValues>();

            // Act:
            var tNormalDate = tClass.Properties[LambdaExtensions.NameOf<DateValues>(x => x.NormalDate)];
            var tTimeFormat = tClass.Properties[LambdaExtensions.NameOf<DateValues>(x => x.TimeFormat)];
            var tDateFormat = tClass.Properties[LambdaExtensions.NameOf<DateValues>(x => x.DateFormat)];
            var tCustomDateFormat = tClass.Properties[LambdaExtensions.NameOf<DateValues>(x => x.CustomDateFormat)];
            var tTimespan = tClass.Properties[LambdaExtensions.NameOf<DateValues>(x => x.Timespan)];

            // Assert:
            Assert.AreEqual(DataType.DateTime, tNormalDate.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.Time, tTimeFormat.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.Date, tDateFormat.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.DateTime, tCustomDateFormat.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(DataType.Duration, tTimespan.Attributes.Get<DataTypeAttribute>().DataType);
        }

        [Test]
        public void ShouldBuildConfigurationForNumberProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<NumberValues>();

            // Act:
            var tNormalNumber = tClass.Properties[LambdaExtensions.NameOf<NumberValues>(x => x.NormalNumber)];
            var tHiddenNumber = tClass.Properties[LambdaExtensions.NameOf<NumberValues>(x => x.HiddenNumber)];
            var tNumberWithRange = tClass.Properties[LambdaExtensions.NameOf<NumberValues>(x => x.NumberWithRange)];
            var tDoubleNumber = tClass.Properties[LambdaExtensions.NameOf<NumberValues>(x => x.DoubleNumber)];
            var tFloatNumberWithPlaces = tClass.Properties[LambdaExtensions.NameOf<NumberValues>(x => x.FloatNumberWithPlaces)];

            // Assert:
            Assert.IsFalse(tHiddenNumber.Attributes.Get<VisibilityAttribute>().IsAllowed);

            var rangeAttr = tNumberWithRange.Attributes.Get<System.ComponentModel.DataAnnotations.RangeAttribute>();
            Assert.AreEqual(-234, rangeAttr.Minimum);
            Assert.AreEqual(234, rangeAttr.Maximum);

            Assert.AreEqual(DataType.Currency, tDoubleNumber.Attributes.Get<DataTypeAttribute>().DataType);
            Assert.AreEqual(5, tFloatNumberWithPlaces.Attributes.Get<DecimalPlacesAttribute>().Places);
        }

        [Test]
        public void ShouldIdentifyPersitenceConfiguration()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate<Product>();

            // Assert:
            Assert.IsTrue(tClass.IsPersistable);
            Assert.AreEqual("ID", tClass.IdProperty.Name);
            Assert.AreEqual("Version", tClass.VersionProperty.Name);
        }


        [Test]
        public void ShouldIdentifyNonPersistableClasses()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            // Act:
            var tClass = (ClassTemplate)templateCache.GetTemplate<MethodSamples>();

            // Assert:
            Assert.IsFalse(tClass.IsPersistable);
            Assert.IsNotNull(tClass.IdProperty);
            Assert.IsNull(tClass.VersionProperty);
        }

        [Test]
        public void ShouldRespectConfigurationFromSuperClasses()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<Employee>();

            // Act:
            var tHidden = tClass.Properties[LambdaExtensions.NameOf<Employee>(x => x.HiddenProperty)];

            // Assert:
            Assert.IsFalse(tHidden.IsVisible);
            Assert.IsFalse(tHidden.Attributes.Get<VisibilityAttribute>().IsAllowed);
        }


    }
}