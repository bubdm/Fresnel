using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Features
{
    [TestFixture()]
    public class ProxyChangeTrackingTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private TestScopeContainer _TestScopeContainer = null;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _TestScopeContainer = new TestScopeContainer();

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var engine = _TestScopeContainer.Resolve<Core.Engine>();
                engine.RegisterDomainAssembly(typeof(MultiType).Assembly);
            }
        }

        [Test]
        public void ShouldReturnCollectionAdditions()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {    
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // This ensures the Collection can be tracked:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.Notes)
                };
                var getResult = controller.GetObjectProperty(getRequest);

                // Act:
                var invokeRequest = CreateInvokeRequestForAddVacationTime(employee);

                var invokeResult1 = controller.InvokeMethod(invokeRequest);
                var invokeResult2 = controller.InvokeMethod(invokeRequest);
                var invokeResult3 = controller.InvokeMethod(invokeRequest);

                // Assert:
                // Each call performs 3 new additions:
                Assert.AreEqual(2, invokeResult1.Modifications.CollectionAdditions.Count());
                Assert.AreEqual(2, invokeResult2.Modifications.CollectionAdditions.Count());
                Assert.AreEqual(2, invokeResult3.Modifications.CollectionAdditions.Count());
            }
        }

        [Test]
        public void ShouldReturnNewlyCreatedObservers()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {   
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // This ensures the Collection can be tracked:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.Notes)
                };
                var getResult = controller.GetObjectProperty(getRequest);

                // Act:
                var invokeRequest = CreateInvokeRequestForAddVacationTime(employee);

                var invokeResult = controller.InvokeMethod(invokeRequest);

                // Assert:
                // 1x Employee, 1x List'<Note>, 2x Notes:
                Assert.AreEqual(4, invokeResult.Modifications.NewObjects.Count());
            }
        }

        [Test]
        public void ShouldReturnPopulatedCollectionProperty()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {   
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                employee.Notes.AddMany(() => _Fixture.Create<Note>(), 3);
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // Act:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.Notes)
                };
                var getResult = controller.GetObjectProperty(getRequest);

                // Assert:
                Assert.IsTrue(getResult.Passed);

                var collectionVM = getResult.ReturnValue as CollectionVM;
                Assert.IsNotNull(collectionVM);

                Assert.AreNotEqual(0, collectionVM.Items.Count());
            }
        }

        [Test]
        public void ShouldSetNonReferenceProperties()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {  
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var multiType = _Fixture.Create<MultiType>();
                var oObject = observerRetriever.GetObserver(multiType) as ObjectObserver;

                // Act:
                var requests = new List<SetPropertyRequest>()
                {
                new SetPropertyRequest() { 
                    ObjectID = oObject.ID, 
                    PropertyName = LambdaExtensions.NameOf<MultiType>(x=> x.A_Char),
                    NonReferenceValue = _Fixture.Create<char>() 
                },
                new SetPropertyRequest() { 
                    ObjectID = oObject.ID, 
                    PropertyName = LambdaExtensions.NameOf<MultiType>(x=> x.A_Double),
                    NonReferenceValue = _Fixture.Create<double>() 
                },
                new SetPropertyRequest() { 
                    ObjectID = oObject.ID, 
                    PropertyName = LambdaExtensions.NameOf<MultiType>(x=> x.An_Int),
                    NonReferenceValue = _Fixture.Create<int>() 
                },
                new SetPropertyRequest() { 
                    ObjectID = oObject.ID, 
                    PropertyName = LambdaExtensions.NameOf<MultiType>(x=> x.A_String),
                    NonReferenceValue = _Fixture.Create<string>() 
                },
            };

                foreach (var request in requests)
                {
                    var setResult = controller.SetProperty(request);
                    Assert.AreEqual(1, setResult.Modifications.PropertyChanges.Count());
                }
            }
        }

        [Test]
        public void ShouldReturnPropertyModifications()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            { 
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // Act:
                var request = new SetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.HiredOn),
                    NonReferenceValue = _Fixture.Create<DateTime>()
                };

                var setResult = controller.SetProperty(request);

                // Assert:
                Assert.AreEqual(1, setResult.Modifications.PropertyChanges.Count());
            }
        }

        [Test]
        public void ShouldReturnObjectTitleModifications()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();
                _TestScopeContainer.Resolve<ObserverCache>().CleanUp(); 
                
                // Act:
                var obj = _TestScopeContainer.Resolve<ObjectWithCtorInjection>();
                obj.ID = Guid.NewGuid();
                var oObject = observerRetriever.GetObserver(obj) as ObjectObserver;

                var request = new SetPropertyRequest()
                {
                    ObjectID = obj.ID,
                    PropertyName = LambdaExtensions.NameOf<ObjectWithCtorInjection>(x => x.Name),
                    NonReferenceValue = _Fixture.Create<string>()
                };

                var setResult = controller.SetProperty(request);

                // Assert:
                Assert.AreEqual(2, setResult.Modifications.ObjectTitleChanges.Count());

                var matchingModification = setResult.Modifications.ObjectTitleChanges.Single(m => m.ObjectID == request.ObjectID);
                Assert.AreEqual(request.NonReferenceValue, matchingModification.Title);
            }
        }

        [Test]
        public void ShouldDetectIntraPropertyCalls()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            { 
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var textValues = _Fixture.Create<TextValues>();
                var oObject = observerRetriever.GetObserver(textValues) as ObjectObserver;

                // Act:
                var request = new SetPropertyRequest()
                {
                    ObjectID = textValues.ID,
                    PropertyName = LambdaExtensions.NameOf<TextValues>(x => x.NormalText),
                    NonReferenceValue = _Fixture.Create<string>()
                };

                var setResult = controller.SetProperty(request);

                // Assert:
                // Some of the text properties are bound to the same value:
                Assert.AreEqual(3, setResult.Modifications.PropertyChanges.Count());
            }
        }

        [Test]
        public void ShouldDetectCorrectModificationsWhenCollectionIsExploreredPartway()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {   
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // Act:

                // Step 1: Modify the collection, before we've started tracking it:
                var invokeRequest = this.CreateInvokeRequestForAddVacationTime(employee);

                var invokeResult1 = controller.InvokeMethod(invokeRequest);
                // As we're not tracking the collection, we're not expecting any new items:
                Assert.AreEqual(0, invokeResult1.Modifications.CollectionAdditions.Count());

                // Step 2: Open the Collection, so that the engine starts tracking it:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.Notes),
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
                // We're expecting 2 new items:
                Assert.AreEqual(2, invokeResult2.Modifications.CollectionAdditions.Count());
            }
        }

        [Test]
        public void ShouldDetectRemoveFromCollection()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var employee = _Fixture.Create<Employee>();
                var oObject = observerRetriever.GetObserver(employee) as ObjectObserver;

                // Act:

                // Step 1: Modify the collection, before we've started tracking it:
                var invokeRequest = this.CreateInvokeRequestForAddVacationTime(employee);

                var invokeResult1 = controller.InvokeMethod(invokeRequest);
                // As we're not tracking the collection, we're not expecting any new items:
                Assert.AreEqual(0, invokeResult1.Modifications.CollectionAdditions.Count());

                // Step 2: Open the Collection, so that the engine starts tracking it:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = employee.ID,
                    PropertyName = LambdaExtensions.NameOf<Employee>(x => x.Notes),
                };
                var getPropertyResponse = controller.GetObjectProperty(getRequest);

                var collectionVM = (CollectionVM)getPropertyResponse.ReturnValue;

                var firstNote = employee.Notes.First();
                var elementToRemove = collectionVM.Items.First();

                // Step 3: Modify the collection, now that the collection's being tracked:
                var removeRequest = new CollectionRemoveRequest()
                {
                    ParentObjectID = employee.ID,
                    CollectionPropertyName = getRequest.PropertyName,
                    ElementID = elementToRemove.ID,
                };
                var removeResult = controller.RemoveItemFromCollection(removeRequest);

                // Assert:
                Assert.AreEqual(1, removeResult.Modifications.CollectionRemovals.Count());
                Assert.IsFalse(employee.Notes.Contains(firstNote));

                var getPropertyResponse2 = controller.GetObjectProperty(getRequest);
                var collectionVM2 = (CollectionVM)getPropertyResponse2.ReturnValue;

                Assert.IsFalse(collectionVM2.Items.Any(c => c.ID == elementToRemove.ID));
            }
        }

        private InvokeMethodRequest CreateInvokeRequestForAddVacationTime(Employee employee)
        {
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = employee.ID,
                MethodName = LambdaExtensions.NameOf<Employee>(x => x.AddVacationTime(DateTime.MinValue, DateTime.MinValue)),
                Parameters = new ParameterVM[] { 
                    new ParameterVM()
                    {
                         InternalName = "lastDayAtWork",
                         State = new ValueStateVM(){ Value = DateTime.Now.AddDays(10) }
                    },
                    new ParameterVM()
                    {
                         InternalName = "firstDayBackAtWork",
                         State = new ValueStateVM(){ Value = DateTime.Now.AddDays(20) }
                    },
                }
            };
            return invokeRequest;
        }
    }
}