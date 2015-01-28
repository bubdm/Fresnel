using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.TypeInfo;

using NUnit.Framework;
using System;
using System.Linq;

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
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

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
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

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

            Assert.GreaterOrEqual(collectionVM.ElementProperties.Count(), visibleProperties.Count());
        }

        [Test()]
        public void ShouldRefreshObject()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:
            var request = new SetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "EnumValue",
                NonReferenceValue = 30,
            };
            var setResult = controller.SetProperty(request);

            // Now check that the server object has the right value:
            var getRequest = new GetObjectRequest()
            {
                ObjectID = poco.ID,
            };
            var refreshedObject = controller.GetObject(getRequest);

            // Assert:
            var propVM = refreshedObject.ReturnValue.Properties.Single(p => p.InternalName == "EnumValue");
            Assert.AreEqual(30, propVM.State.Value);
        }

        [Test()]
        public void ShouldAddNewItemToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionRequest()
            {
                CollectionID = collectionVM.ID,
                ElementTypeName = oObject.Template.FullName
            };

            var response = controller.AddItemToCollection(addRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(1, response.Modifications.NewObjects.Count());
            Assert.AreEqual(1, response.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            var oChild = observerCache.GetObserverById(response.Modifications.NewObjects.First().ID);
            Assert.IsTrue(poco.ChildObjects.Contains(oChild.RealObject));
        }

        [Test()]
        public void ShouldAddExistingItemToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            var child = new SampleModel.Objects.PocoObject();
            child.ID = Guid.NewGuid();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionRequest()
            {
                CollectionID = collectionVM.ID,
                ElementID = oChild.ID,
            };

            var response = controller.AddItemToCollection(addRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(0, response.Modifications.NewObjects.Count());
            Assert.AreEqual(1, response.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            Assert.IsTrue(poco.ChildObjects.Contains(oChild.RealObject));
        }


        [Test()]
        public void ShouldRemoveItemFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            var child = poco.ChildObjects.First();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var removeRequest = new CollectionRequest()
            {
                CollectionID = collectionVM.ID,
                ElementID = oChild.ID,
            };

            var response = controller.RemoveItemFromCollection(removeRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(1, response.Modifications.CollectionRemovals.Count());

            // Check that the domain object has changed:
            Assert.IsFalse(poco.ChildObjects.Contains(child));
        }


        [Test()]
        public void ShouldInvokeMethodWithMultipleParameters()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = new SampleModel.MethodTests();
            obj.ID = Guid.NewGuid();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var request = new InvokeMethodRequest()
            {
                ObjectID = obj.ID,
                MethodName = "MethodWithValueParameters",
                Parameters = new SettableMemberVM[] { 
                    new SettableMemberVM(){ InternalName = "aString", State = new ValueStateVM() { Value = "123"} },
                    new SettableMemberVM(){ InternalName = "aNumber", State = new ValueStateVM() { Value = 123} },
                    new SettableMemberVM(){ InternalName = "aDate", State = new ValueStateVM() { Value = DateTime.Now} },
                 }
            };

            var response = controller.InvokeMethod(request);

            // Assert:
            Assert.IsTrue(response.Passed);

        }
    }
}