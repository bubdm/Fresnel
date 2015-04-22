using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
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
    public class ExplorerControllerTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldReturnObjectProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = _Fixture.Create<MultiType>();

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.An_Object)
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            Assert.IsTrue(getResult.Passed);
            Assert.IsNotNull(getResult.ReturnValue);
        }

        [Test]
        public void ShouldReturnCorrectHeadersForCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();


            var obj = _Fixture.Create<MultiType>();
            obj.A_Collection.AddMany(() => _Fixture.Create<BooleanValues>(), 5);

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection)
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // There should be a Header for each viewable property in the collection's Element type:
            var elementType = typeof(BooleanValues);
            var template = (ClassTemplate)templateCache.GetTemplate(elementType);

            var visibleProperties = template.Properties.VisibleOnly;

            Assert.GreaterOrEqual(collectionVM.ElementProperties.Count(), visibleProperties.Count());
        }

        [Test]
        public void ShouldRefreshObject()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();


            var obj = _Fixture.Create<MultiType>();

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Act:
            var request = new SetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_String),
                NonReferenceValue = _Fixture.Create<string>(),
            };
            var setResult = controller.SetProperty(request);

            // Now check that the server object has the right value:
            var getRequest = new GetObjectRequest()
            {
                ObjectID = obj.ID,
            };
            var refreshedObject = controller.GetObject(getRequest);

            // Assert:
            var propVM = refreshedObject.ReturnValue.Properties.Single(p => p.InternalName == request.PropertyName);
            Assert.AreEqual(request.NonReferenceValue, propVM.State.Value);
        }

        [Test]
        public void ShouldAddNewItemToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();


            var obj = _Fixture.Create<MultiType>();
            obj.A_Collection.AddMany(() => _Fixture.Create<BooleanValues>(), 5);

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Make sure we start tracking the collection:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection)
            };
            var getResult = controller.GetObjectProperty(getRequest);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionAddNewRequest()
            {
                ParentObjectID = obj.ID,
                CollectionPropertyName = getRequest.PropertyName,
                ElementTypeName = typeof(BooleanValues).FullName
            };

            var addResponse = controller.AddNewItemToCollection(addRequest);

            // Assert:
            Assert.IsTrue(addResponse.Passed);
            Assert.AreEqual(1, addResponse.Modifications.NewObjects.Count());
            Assert.AreEqual(1, addResponse.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            var oChild = observerCache.GetObserverById(addResponse.Modifications.NewObjects.First().ID);
            Assert.IsTrue(obj.A_Collection.Contains(oChild.RealObject));
        }

        [Test]
        public void ShouldAddExistingItemToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();


            var obj = _Fixture.Create<MultiType>();
            obj.A_Collection.AddMany(() => _Fixture.Create<BooleanValues>(), 5);

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            var child = _Fixture.Create<BooleanValues>();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection)
            };
            var getResult = controller.GetObjectProperty(getRequest);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionAddRequest()
            {
                ParentObjectID = obj.ID,
                CollectionPropertyName = getRequest.PropertyName,
                ElementIDs = new Guid[] { oChild.ID },
            };

            var response = controller.AddItemsToCollection(addRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(0, response.Modifications.NewObjects.Count());
            Assert.AreEqual(1, response.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            Assert.IsTrue(obj.A_Collection.Contains(oChild.RealObject));
        }

        [Test]
        public void ShouldCreateNewPropertyObjectWithParentAsCtorArg()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = _Fixture.Create<MultiType>();
            obj.A_Collection.AddMany(() => _Fixture.Create<BooleanValues>(), 5);

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Make sure we start tracking the property:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.An_Object)
            };
            var getResult = controller.GetObjectProperty(getRequest);

            // Act:
            var setRequest = new CreateAndSetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = getRequest.PropertyName,
                ClassTypeName = typeof(TextValues).FullName
            };

            var setResponse = controller.CreateAndSetProperty(setRequest);

            // Assert:
            Assert.IsTrue(setResponse.Passed);
            Assert.AreEqual(1, setResponse.Modifications.NewObjects.Count());
            Assert.AreEqual(1, setResponse.Modifications.PropertyChanges.Count());

            // Check that the domain object has changed:
            var oChild = observerCache.GetObserverById(setResponse.Modifications.PropertyChanges.First().State.ReferenceValueID.Value);
            Assert.AreEqual(obj.An_Object, oChild.RealObject);
        }

        [Test]
        public void ShouldRemoveItemFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();


            var obj = _Fixture.Create<MultiType>();
            obj.A_Collection.AddMany(() => _Fixture.Create<BooleanValues>(), 5);

            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            var child = obj.A_Collection.First();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection)
            };
            var getResult = controller.GetObjectProperty(getRequest);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var removeRequest = new CollectionRemoveRequest()
            {
                ParentObjectID = obj.ID,
                CollectionPropertyName = getRequest.PropertyName,
                ElementID = oChild.ID,
            };

            var response = controller.RemoveItemFromCollection(removeRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(1, response.Modifications.CollectionRemovals.Count());

            // Check that the domain object has changed:
            Assert.IsFalse(obj.A_Collection.Contains(child));
        }

        [Test]
        public void ShouldAllowCreateOnCompositeProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = _Fixture.Create<Order>();
            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Act:
            var getRequest = new GetObjectRequest()
            {
                ObjectID = obj.ID,
            };
            var getResponse = controller.GetObject(getRequest);

            var propName = LambdaExtensions.NameOf<Order>(x => x.OrderItems);
            var objectVM = getResponse.ReturnValue;
            var propVM = objectVM.Properties.Single(p => p.InternalName == propName);

            // Assert:
            Assert.IsTrue(propVM.State.Create.IsEnabled);
        }

        [Test]
        public void ShouldNotAllowCreateOnAggregateProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = _Fixture.Create<Order>();
            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            // Act:
            var getRequest = new GetObjectRequest()
            {
                ObjectID = obj.ID,
            };
            var getResponse = controller.GetObject(getRequest);

            var propName = LambdaExtensions.NameOf<Order>(x => x.DeliverTo);
            var objectVM = getResponse.ReturnValue;
            var propVM = objectVM.Properties.Single(p => p.InternalName == propName);

            // Assert:
            Assert.IsFalse(propVM.State.Create.IsEnabled);
        }
    }
}