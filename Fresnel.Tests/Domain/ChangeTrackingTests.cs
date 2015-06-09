using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ChangeTrackingTests
    {
        private TestScopeContainer _TestScopeContainer = new TestScopeContainer();
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldHaveDefaultChangeStatus()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                observerCache.CleanUp();

                var person = _Fixture.Create<Person>();

                // Act:
                var observer = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                Assert.AreSame(person, observer.RealObject);

                // Assert:
                Assert.IsTrue(observer.ChangeTracker.IsTransient);
                Assert.IsFalse(observer.ChangeTracker.IsDirty);
            }
        }

        [Test]
        public void ShouldDetectPropertyChanges()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var setCommand = _TestScopeContainer.Resolve<SetPropertyCommand>();

                observerCache.CleanUp();
                var person = _Fixture.Create<Person>();

                var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                Assert.AreSame(person, oObject.RealObject);

                var propName = LambdaExtensions.NameOf<Person>(x => x.FirstName);
                var oProp = oObject.Properties[propName];
                var oValue = observerCache.GetObserver(_Fixture.Create<string>(), typeof(string));

                // Act:
                setCommand.Invoke(oProp, oValue);

                // Assert:
                Assert.IsTrue(oObject.ChangeTracker.IsDirty);
            }
        }

        [Test]
        public void ShouldDetectNewInstances()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var createCommand = _TestScopeContainer.Resolve<CreateObjectCommand>();

                // Act:
                var personType = typeof(Person);
                var oObject = createCommand.Invoke(personType, null);

                // Assert:
                Assert.IsNotNull(oObject);
                Assert.IsInstanceOf<Person>(oObject.RealObject);
            }
        }

        [Test]
        public void ShouldDetectAddToCollection()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var getCommand = _TestScopeContainer.Resolve<GetPropertyCommand>();
                var addCommand = _TestScopeContainer.Resolve<AddToCollectionCommand>();

                observerCache.CleanUp();
                var person = _Fixture.Create<Person>();

                var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                Assert.AreSame(person, oObject.RealObject);

                var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
                var oProp = (ObjectPropertyObserver)oObject.Properties[propName];
                var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

                // Act:
                var newRole = _Fixture.Create<Employee>();
                var oNewRole = (ObjectObserver)observerCache.GetObserver(newRole, newRole.GetType());

                var result = addCommand.Invoke(oProp, oCollection, oNewRole);

                // Assert:
                Assert.IsTrue(oNewRole.ChangeTracker.IsDirty);
                Assert.IsTrue(oNewRole.ChangeTracker.IsMarkedForAddition);
                Assert.IsTrue(oCollection.ChangeTracker.IsDirty);
                Assert.IsTrue(oObject.ChangeTracker.HasDirtyObjectGraph);
            }
        }

        [Test]
        public void ShouldRevertDirtyStatusWhenTransientObjectIsAddedThenRemoved()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var getCommand = _TestScopeContainer.Resolve<GetPropertyCommand>();
                var removeCommand = _TestScopeContainer.Resolve<RemoveFromCollectionCommand>();

                observerCache.CleanUp();
                var person = _Fixture.Create<Person>();
                person.Roles.Add(_Fixture.Create<Employee>());
                person.Roles.Add(_Fixture.Create<Customer>());
                person.Roles.Add(_Fixture.Create<Supplier>());

                var oPerson = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                var rolesPropName = LambdaExtensions.NameOf<Person>(x => x.Roles);
                var oRolesProp = (ObjectPropertyObserver)oPerson.Properties[rolesPropName];
                var oRoles = (CollectionObserver)getCommand.Invoke(oRolesProp);

                // Act:
                var childObject = person.Roles.Last();
                var oChildObject = (ObjectObserver)observerCache.GetObserver(childObject, childObject.GetType());

                var result = removeCommand.Invoke(oRolesProp, oRoles, oChildObject);

                // Assert:
                Assert.IsTrue(oChildObject.ChangeTracker.IsTransient);
                Assert.IsFalse(oChildObject.ChangeTracker.IsDirty);
                Assert.IsFalse(oChildObject.ChangeTracker.IsMarkedForRemoval, "The removed child is transient AND detached from the collection, so doesn't need saving");
                Assert.IsFalse(oRoles.ChangeTracker.IsDirty);
                Assert.IsFalse(oPerson.ChangeTracker.HasDirtyObjectGraph);
            }
        }

        [Test]
        public void ShouldLeaveCollectionUnchangedWhenAddingAndRemovingNewInstance()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var getCommand = _TestScopeContainer.Resolve<GetPropertyCommand>();
                var createCommand = _TestScopeContainer.Resolve<CreateObjectCommand>();
                var addCommand = _TestScopeContainer.Resolve<AddToCollectionCommand>();
                var removeCommand = _TestScopeContainer.Resolve<RemoveFromCollectionCommand>();

                observerCache.CleanUp();
                var person = _Fixture.Create<Person>();
                person.Roles.Add(_Fixture.Create<Employee>());
                person.Roles.Add(_Fixture.Create<Customer>());
                person.Roles.Add(_Fixture.Create<Supplier>());

                var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
                var oProp = (ObjectPropertyObserver)oObject.Properties[propName];
                var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

                // Act:
                var oNewItem = (ObjectObserver)createCommand.Invoke(typeof(Employee), null);
                ((Employee)oNewItem.RealObject).ID = Guid.NewGuid();

                var addResult = addCommand.Invoke(oProp, oCollection, oNewItem);
                var removeResult = removeCommand.Invoke(oProp, oCollection, oNewItem);

                // Assert:
                Assert.IsTrue(oNewItem.ChangeTracker.IsTransient);
                Assert.IsFalse(oNewItem.ChangeTracker.IsMarkedForAddition);
                Assert.IsFalse(oNewItem.ChangeTracker.IsMarkedForRemoval);
                Assert.IsFalse(oCollection.ChangeTracker.IsDirty);
                Assert.IsFalse(oObject.ChangeTracker.HasDirtyObjectGraph);
            }
        }

        [Test]
        public void ShouldDetectChangesToLargeObjectGraph()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var getCommand = _TestScopeContainer.Resolve<GetPropertyCommand>();
                var setCommand = _TestScopeContainer.Resolve<SetPropertyCommand>();

                observerCache.CleanUp();
                var person = _Fixture.Create<Person>();

                var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
                var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
                var oProp = (ObjectPropertyObserver)oObject.Properties[propName];

                var iterations = 10000;

                // Act:
                for (var i = 0; i < iterations; i++)
                {
                    var employee = new Employee()
                    {
                        ID = Guid.NewGuid()
                    };
                    person.Roles.Add(employee);
                }

                var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

                // Act:
                var namePropName = LambdaExtensions.NameOf<Person>(x => x.FirstName);
                var oNameProp = oObject.Properties[namePropName];
                var oValue = observerCache.GetObserver(_Fixture.Create<string>(), typeof(string));
                setCommand.Invoke(oNameProp, oValue);

                // Assert:
                oCollection = (CollectionObserver)getCommand.Invoke(oProp);
                Assert.AreEqual(iterations, oCollection.GetItems().Cast<object>().Count());
            }
        }

        [Test]
        public void ShouldCascadeDirtyStateChanges()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var createCommand = _TestScopeContainer.Resolve<CreateObjectCommand>();
                var getCommand = _TestScopeContainer.Resolve<GetPropertyCommand>();
                var addCommand = _TestScopeContainer.Resolve<AddToCollectionCommand>();

                observerCache.CleanUp();
                var oOrder = (ObjectObserver)createCommand.Invoke(typeof(Order), null);
                var oProp = (ObjectPropertyObserver)oOrder.Properties[LambdaExtensions.NameOf<Order>(x => x.OrderItems)];
                var oOrderItems = (CollectionObserver)getCommand.Invoke(oProp);

                // Add a new OrderItem:
                var oOrderItem = (ObjectObserver)createCommand.Invoke(typeof(OrderItem), null);
                var oAddedItem = (ObjectObserver)addCommand.Invoke(oProp, oOrderItems, oOrderItem);

                // The chain of objects should be dirty:
                Assert.IsTrue(oOrderItem.ChangeTracker.IsTransient);
                Assert.IsTrue(oOrderItem.ChangeTracker.IsMarkedForAddition);
                Assert.IsTrue(oOrderItems.ChangeTracker.HasDirtyObjectGraph);
                Assert.IsTrue(oOrder.ChangeTracker.HasDirtyObjectGraph);

                var dirtyNotifier = _TestScopeContainer.Resolve<DirtyObjectNotifier>();
                dirtyNotifier.ObjectIsNoLongerDirty(oOrderItem);

                // Now the chain of objects should be clean:
                Assert.IsTrue(oOrderItem.ChangeTracker.IsTransient);
                Assert.IsFalse(oOrderItem.ChangeTracker.IsMarkedForAddition);
                Assert.IsFalse(oOrderItems.ChangeTracker.HasDirtyObjectGraph);
                Assert.IsFalse(oOrder.ChangeTracker.HasDirtyObjectGraph);

            }
        }
    }
}