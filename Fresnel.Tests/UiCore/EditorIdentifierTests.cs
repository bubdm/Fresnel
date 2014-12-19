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
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Proxies;
using System.ComponentModel;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.TypeInfo;
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
            Assert.AreEqual("string", charVM.Info.Name);
            Assert.AreEqual("string", stringVM.Info.Name);
            Assert.AreEqual("string", multiLineVM.Info.Name);
            Assert.AreEqual("string", passwordVM.Info.Name);
        }

    }

}

