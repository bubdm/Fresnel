using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ExplorerControllerTests
    {
        [Test]
        public void ShouldReturnObjectProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var fixture = new Fixture();
            var category = fixture.Create<Category>();

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Products)
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

            var fixture = new Fixture();
            var category = fixture.Create<Category>();
            category.Products.AddMany(() => fixture.Create<Product>(), 5);

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Products)
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // There should be a Header for each viewable property in the collection's Element type:
            var template = (ClassTemplate)templateCache.GetTemplate(category.GetType());

            var visibleProperties = template.Properties.Values.Where(p => !p.IsFrameworkMember && p.IsVisible);

            Assert.GreaterOrEqual(collectionVM.ElementProperties.Count(), visibleProperties.Count());
        }

        [Test]
        public void ShouldRefreshObject()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var fixture = new Fixture();
            var category = fixture.Create<Category>();

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            // Act:
            var request = new SetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Image),
                NonReferenceValue = fixture.Create<string>(),
            };
            var setResult = controller.SetProperty(request);

            // Now check that the server object has the right value:
            var getRequest = new GetObjectRequest()
            {
                ObjectID = category.ID,
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

            var fixture = new Fixture();
            var category = fixture.Create<Category>();
            category.Products.AddMany(() => fixture.Create<Product>(), 5);

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Products)
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionAddNewRequest()
            {
                CollectionID = collectionVM.ID,
                ElementTypeName = oObject.Template.FullName
            };

            var response = controller.AddNewItemToCollection(addRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(1, response.Modifications.NewObjects.Count());
            Assert.AreEqual(1, response.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            var oChild = observerCache.GetObserverById(response.Modifications.NewObjects.First().ID);
            Assert.IsTrue(category.Products.Contains(oChild.RealObject));
        }

        [Test]
        public void ShouldAddExistingItemToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var fixture = new Fixture();
            var category = fixture.Create<Category>();
            category.Products.AddMany(() => fixture.Create<Product>(), 5);

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            var child = new Product();
            child.ID = Guid.NewGuid();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Products)
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var addRequest = new CollectionAddRequest()
            {
                CollectionID = collectionVM.ID,
                ElementIDs = new Guid[] { oChild.ID },
            };

            var response = controller.AddItemsToCollection(addRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(0, response.Modifications.NewObjects.Count());
            Assert.AreEqual(1, response.Modifications.CollectionAdditions.Count());

            // Check that the domain object has changed:
            Assert.IsTrue(category.Products.Contains(oChild.RealObject));
        }


        [Test]
        public void ShouldRemoveItemFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var fixture = new Fixture();
            var category = fixture.Create<Category>();
            category.Products.AddMany(() => fixture.Create<Product>(), 5);

            var oObject = observerCache.GetObserver(category) as ObjectObserver;

            var child = category.Products.First();
            var oChild = observerCache.GetObserver(child) as ObjectObserver;

            // Make sure we start tracking the collection:
            var request = new GetPropertyRequest()
            {
                ObjectID = category.ID,
                PropertyName = LambdaExtensions.NameOf<Category>(x => x.Products)
            };
            var getResult = controller.GetObjectProperty(request);

            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // Act:
            var removeRequest = new CollectionRemoveRequest()
            {
                CollectionID = collectionVM.ID,
                ElementID = oChild.ID,
            };

            var response = controller.RemoveItemFromCollection(removeRequest);

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.AreEqual(1, response.Modifications.CollectionRemovals.Count());

            // Check that the domain object has changed:
            Assert.IsFalse(category.Products.Contains(child));
        }

    }
}