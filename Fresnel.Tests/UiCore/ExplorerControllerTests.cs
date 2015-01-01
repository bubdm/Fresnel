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
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.UiCore.Commands;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ExplorerControllerTests
    {

        [Test()]
        public void ShouldReturnObjectProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            Assert.IsTrue(getResult.Passed);
            Assert.IsNotNull(getResult.ReturnValue);
        }

        [Test()]
        public void ShouldReturnCorrectHeadersForCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // There should be a Header for each viewable property in the collection's Element type:
            var tPoco = (ClassTemplate)templateCache.GetTemplate(poco.GetType());

            var visibleProperties = tPoco.Properties.Values.Where(p => !p.IsFrameworkMember && p.IsVisible);

            Assert.GreaterOrEqual(collectionVM.ColumnHeaders.Count(), visibleProperties.Count());
        }

        [Test()]
        public void ShouldReturnCollectionAdditions()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:          
            var request = new InvokeMethodRequest()
            {
                ObjectID = poco.ID,
                MethodName = "AddSomeChildObjects",
            };

            var invokeResult = controller.InvokeMethod(request);

            // Assert:
            Assert.AreEqual(3, invokeResult.Modifications.CollectionAdditions.Count());
        }

        [Test()]
        public void ShouldReturnPopulatedCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var pocoProxy = proxyCache.GetProxy(poco);
            pocoProxy.AddSomeChildObjects();

            // Act:          
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            Assert.IsTrue(getResult.Passed);

            var collectionVM = getResult.ReturnValue as CollectionVM;
            Assert.IsNotNull(collectionVM);

            Assert.AreNotEqual(0, collectionVM.Items.Count());
        }


        [Test()]
        public void ShouldReturnPropertyModifications()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:          
            var request = new SetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "NormalText",
                NonReferenceValue = "1234"
            };

            var setResult = controller.SetProperty(request);

            // Assert:
            Assert.AreEqual(1, setResult.Modifications.PropertyChanges.Count());
        }
    }

}

