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


using System.ComponentModel;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Types;
using Envivo.Fresnel.UiCore.Objects;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class EditorIdentifierTests
    {

        [Test()]
        public void ShouldIdentifyTextValues()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractPropertyVmBuilder>();

            var textValues = new SampleModel.BasicTypes.TextValues();
            var oObject = (ObjectObserver)observerCache.GetObserver(textValues);

            // Act:
            var charVM = vmBuilder.BuildFor(oObject.Properties["NormalChar"]);

            var stringVM = vmBuilder.BuildFor(oObject.Properties["NormalText"]);

            var multiLineVM = vmBuilder.BuildFor(oObject.Properties["MultiLineText"]);

            var passwordVM = vmBuilder.BuildFor(oObject.Properties["PasswordText"]);

            // Assert:
            Assert.AreEqual(InputControlTypes.Text, charVM.Info.PreferredControl);
            Assert.AreEqual(InputControlTypes.Text, stringVM.Info.PreferredControl);
            Assert.AreEqual(InputControlTypes.TextArea, multiLineVM.Info.PreferredControl);
            Assert.AreEqual(InputControlTypes.Password, passwordVM.Info.PreferredControl);
        }


        [Test()]
        public void ShouldIdentifyEnumPropertyTypes()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractPropertyVmBuilder>();

            var obj = new SampleModel.BasicTypes.EnumValues();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var enumVM = vmBuilder.BuildFor(oObject.Properties["EnumValue"]);

            var bitwiseEnumVM = vmBuilder.BuildFor(oObject.Properties["EnumSwitches"]);

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

        [Test()]
        public void ShouldIdentifyPropertyTypesForCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractObjectVMBuilder>();

            var col = new Collection<SampleModel.Objects.PocoObject>();
            var oColl = (ObjectObserver)observerCache.GetObserver(col);

            // Act:
            var collVM = (CollectionVM)vmBuilder.BuildFor(oColl);

            // Assert:
            foreach (var prop in collVM.ElementProperties)
            {
                if (prop.Info != null)
                {
                    Assert.AreNotEqual(InputControlTypes.None, prop.Info.PreferredControl);
                }
            }
        }
    }

}

