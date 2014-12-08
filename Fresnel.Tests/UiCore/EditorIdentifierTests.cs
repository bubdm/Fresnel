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
using Envivo.Fresnel.UiCore.Editing;

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
            var identifier = container.Resolve<EditorTypeIdentifier>();

            var textValues = new SampleModel.BasicTypes.TextValues();
            var oObject = (ObjectObserver)observerCache.GetObserver(textValues);
            
            // Act:
            var char_ = identifier.DetermineEditorFor(oObject.Properties["NormalChar"]);

            var string_ = identifier.DetermineEditorFor(oObject.Properties["NormalText"]);

            var multiLineText = identifier.DetermineEditorFor(oObject.Properties["MultiLineText"]);

            var passwordText = identifier.DetermineEditorFor(oObject.Properties["PasswordText"]);

            // Assert:
            Assert.AreEqual(EditorType.Character, char_);
            Assert.AreEqual(EditorType.String, string_);
            Assert.AreEqual(EditorType.MultiLineText, multiLineText);
            Assert.AreEqual(EditorType.Password, passwordText);
        }

    }

}

