using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ProxyChangeTrackingTests
    {
        [Test]
        public void ShouldReturnCollectionAdditions()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // This ensures the Collection can be tracked:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getResult = controller.GetObjectProperty(getRequest);

            // Act:
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = poco.ID,
                MethodName = "AddSomeChildObjects",
            };

            var invokeResult1 = controller.InvokeMethod(invokeRequest);

            var invokeResult2 = controller.InvokeMethod(invokeRequest);

            var invokeResult3 = controller.InvokeMethod(invokeRequest);

            // Assert:
            // Each call performs 3 new additions:
            Assert.AreEqual(3, invokeResult1.Modifications.CollectionAdditions.Count());
            Assert.AreEqual(3, invokeResult2.Modifications.CollectionAdditions.Count());
            Assert.AreEqual(3, invokeResult3.Modifications.CollectionAdditions.Count());
        }
    
        [Test]
        public void ShouldReturnNewlyCreatedObservers()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // This ensures the Collection can be tracked:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getResult = controller.GetObjectProperty(getRequest);

            // Act:
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = poco.ID,
                MethodName = "AddSomeChildObjects",
            };

            var invokeResult = controller.InvokeMethod(invokeRequest);

            // Assert:
            Assert.AreEqual(3, invokeResult.Modifications.NewObjects.Count());
        }

        [Test]
        public void ShouldReturnPopulatedCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            poco.AddSomeChildObjects();

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

        [Test]
        public void ShouldSetNonReferenceProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.BasicTypes.MultiType();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:
            var requests = new List<SetPropertyRequest>()
            {
                new SetPropertyRequest() { ObjectID = oObject.ID, PropertyName = "A_Char", NonReferenceValue = "X" },
                new SetPropertyRequest() { ObjectID = oObject.ID, PropertyName = "A_Double", NonReferenceValue = "123.45" },
                new SetPropertyRequest() { ObjectID = oObject.ID, PropertyName = "An_Int", NonReferenceValue = "1234" },
                new SetPropertyRequest() { ObjectID = oObject.ID, PropertyName = "A_String", NonReferenceValue = "ABC123" },
            };

            foreach (var request in requests)
            {
                var setResult = controller.SetProperty(request);
                Assert.AreEqual(1, setResult.Modifications.PropertyChanges.Count());
            }
        }

        [Test]
        public void ShouldReturnPropertyModifications()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

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

        [Test]
        public void ShouldReturnObjectTitleModifications()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.Objects.DependencyAwareObject).Assembly);

            // Act:
            var obj = container.Resolve<SampleModel.Objects.DependencyAwareObject>();
            obj.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(obj) as ObjectObserver;

            var request = new SetPropertyRequest()
            {
                ObjectID = obj.ID,
                PropertyName = "Name",
                NonReferenceValue = "Test " + Environment.TickCount.ToString()
            };

            var setResult = controller.SetProperty(request);

            // Assert:
            Assert.AreEqual(1, setResult.Modifications.ObjectTitleChanges.Count());
            Assert.AreEqual(request.NonReferenceValue, setResult.Modifications.ObjectTitleChanges.First().Title);
        }

        [Test]
        public void ShouldDetectIntraPropertyCalls()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.BasicTypes.TextValues();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:
            var request = new SetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "TextWithMaximumSize",
                NonReferenceValue = "1234"
            };

            var setResult = controller.SetProperty(request);

            // Assert:
            // All of the text properties are bound to the same value:
            Assert.AreEqual(8, setResult.Modifications.PropertyChanges.Count());
        }

        [Test]
        public void ShouldDetectCorrectModificationsWhenCollectionIsExploreredPartway()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:

            // Step 1: Modify the collection, before we've started tracking it:
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = poco.ID,
                MethodName = "AddSomeChildObjects",
            };

            var invokeResult1 = controller.InvokeMethod(invokeRequest);
            // As we're not tracking the collection, we're not expecting any new items:
            Assert.AreEqual(0, invokeResult1.Modifications.CollectionAdditions.Count());

            // Step 2: Open the Collection, so that the engine starts tracking it:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getPropertyResponse = controller.GetObjectProperty(getRequest);

            var getObjectRequest = new GetObjectRequest()
            {
                ObjectID = getPropertyResponse.ReturnValue.ID
            };
            var getObjectResult = controller.GetObject(getObjectRequest);

            // Step 3: Modify the collection, now that the collection's being tracked:
            var invokeResult2 = controller.InvokeMethod(invokeRequest);

            // Assert:
            // We're expecting 3 new items:
            Assert.AreEqual(3, invokeResult2.Modifications.CollectionAdditions.Count());
        }


        [Test]
        public void ShouldDetectRemoveFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:

            // Step 1: Modify the collection, before we've started tracking it:
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = poco.ID,
                MethodName = "AddSomeChildObjects",
            };

            var invokeResult1 = controller.InvokeMethod(invokeRequest);
            // As we're not tracking the collection, we're not expecting any new items:
            Assert.AreEqual(0, invokeResult1.Modifications.CollectionAdditions.Count());

            // Step 2: Open the Collection, so that the engine starts tracking it:
            var getRequest = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };
            var getPropertyResponse = controller.GetObjectProperty(getRequest);

            var collectionVM = (CollectionVM)getPropertyResponse.ReturnValue;

            var firstPocoChild = poco.ChildObjects.First();
            var elementToRemove = collectionVM.Items.First();

            // Step 3: Modify the collection, now that the collection's being tracked:
            var removeRequest = new CollectionRemoveRequest()
            {
                CollectionID = collectionVM.ID,
                ElementID = elementToRemove.ID,
            };
            var removeResult = controller.RemoveItemFromCollection(removeRequest);

            // Assert:
            Assert.AreEqual(1, removeResult.Modifications.CollectionRemovals.Count());
            Assert.IsFalse(poco.ChildObjects.Contains(firstPocoChild));

            var getPropertyResponse2 = controller.GetObjectProperty(getRequest);
            var collectionVM2 = (CollectionVM)getPropertyResponse2.ReturnValue;

            Assert.IsFalse(collectionVM2.Items.Any(c => c.ID == elementToRemove.ID));
        }

    }
}