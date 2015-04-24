using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Features
{
    [TestFixture()]
    public class EditorIdentifierTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldIdentifyPropertyTypes()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<PropertyVmBuilder>();


            var obj = _Fixture.Create<MultiType>();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var boolVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_Boolean)]);
            var charVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_Char)]);
            var stringVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_String)]);
            var intVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.An_Int)]);
            var doubleVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_Double)]);
            var floatVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_Float)]);
            var dateTimeVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_DateTime)]);
            var dateTimeOffsetVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<MultiType>(x => x.A_DateTimeOffset)]);

            // Assert:
            Assert.AreEqual(UiControlType.Radio, boolVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Text, charVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Text, stringVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Number, intVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Number, doubleVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Number, floatVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.DateTimeLocal, dateTimeVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.DateTimeLocal, dateTimeOffsetVM.Info.PreferredControl);
        }

        [Test]
        public void ShouldIdentifyTextValues()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<PropertyVmBuilder>();


            var obj = _Fixture.Create<TextValues>();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var charVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<TextValues>(x => x.NormalChar)]);
            var stringVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<TextValues>(x => x.NormalText)]);
            var multiLineVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<TextValues>(x => x.MultiLineText)]);
            var passwordVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<TextValues>(x => x.PasswordText)]);

            // Assert:
            Assert.AreEqual(UiControlType.Text, charVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Text, stringVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.TextArea, multiLineVM.Info.PreferredControl);
            Assert.AreEqual(UiControlType.Password, passwordVM.Info.PreferredControl);
        }

        [Test]
        public void ShouldIdentifyEnumPropertyTypes()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<PropertyVmBuilder>();


            var obj = _Fixture.Create<EnumValues>();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var enumVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<EnumValues>(x => x.EnumValue)]);
            var bitwiseEnumVM = vmBuilder.BuildFor(oObject.Properties[LambdaExtensions.NameOf<EnumValues>(x => x.EnumSwitches)]);

            // Assert:
            Assert.IsFalse(((EnumVM)enumVM.Info).IsBitwiseEnum);
            foreach (var enumItemVM in ((EnumVM)enumVM.Info).Items)
            {
                Assert.AreNotEqual(enumItemVM.Name, enumItemVM.Value);
            }

            Assert.IsTrue(((EnumVM)bitwiseEnumVM.Info).IsBitwiseEnum);
            foreach (var enumItemVM in ((EnumVM)bitwiseEnumVM.Info).Items)
            {
                Assert.AreNotEqual(enumItemVM.Name, enumItemVM.Value);
            }
        }

        [Test]
        public void ShouldIdentifyPropertyTypesForCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractObjectVmBuilder>();


            var col = _Fixture.Create<Collection<Product>>();
            var oColl = (ObjectObserver)observerCache.GetObserver(col);

            // Act:
            var collVM = (CollectionVM)vmBuilder.BuildFor(oColl);

            // Assert:
            foreach (var prop in collVM.ElementProperties)
            {
                if (prop.Info != null)
                {
                    Assert.AreNotEqual(UiControlType.None, prop.Info.PreferredControl);
                }
            }
        }

        [Test]
        public void ShouldIdentifyMethodParameterValues()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MethodSamples).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractObjectVmBuilder>();

            var obj = container.Resolve<MethodSamples>();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var vm = vmBuilder.BuildFor(oObject);

            // Assert:
            var methodName = LambdaExtensions.NameOf<MethodSamples>(x => x.MethodWithValueParameters(null, null, 0, DateTime.MinValue));
            var methodWithParams = vm.Methods.Single(m => m.InternalName == methodName);

            Assert.IsTrue(methodWithParams.Parameters.Any(p => p.State.ValueType != null));
            Assert.IsTrue(methodWithParams.Parameters.Any(p => p.Info != null));
            Assert.IsTrue(methodWithParams.Parameters.Any(p => p.Description != null));
        }
    }
}