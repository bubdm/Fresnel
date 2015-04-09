using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.SampleModel.Northwind;
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
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldHaveDefaultChangeStatus()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var person = _Fixture.Create<Person>();

            // Act:
            var observer = (ObjectObserver)observerCache.GetObserver(person, person.GetType());

            // Assert:
            Assert.IsFalse(observer.ChangeTracker.IsDirty);
        }

        [Test]
        public void ShouldDetectPropertyChanges()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var person = _Fixture.Create<Person>();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var propName = LambdaExtensions.NameOf<Person>(x => x.FirstName);
            var oProp = oObject.Properties[propName];
            var oValue = observerCache.GetObserver(_Fixture.Create<string>(), typeof(string));

            // Act:
            setCommand.Invoke(oProp, oValue);

            // Assert:
            Assert.IsTrue(oObject.ChangeTracker.IsDirty);
        }

        [Test]
        public void ShouldDetectNewInstances()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var createCommand = container.Resolve<CreateObjectCommand>();

            // Act:
            var personType = typeof(Person);
            var oObject = createCommand.Invoke(personType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            Assert.IsInstanceOf<Person>(oObject.RealObject);
        }

        [Test]
        public void ShouldDetectAddToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var person = _Fixture.Create<Person>();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
            var oProp = oObject.Properties[propName];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var newRole = _Fixture.Create<Employee>();
            var oNewRole = (ObjectObserver)observerCache.GetObserver(newRole, newRole.GetType());

            var result = addCommand.Invoke(oCollection, oNewRole);

            // Assert:
            Assert.IsTrue(oNewRole.ChangeTracker.IsDirty);
            Assert.IsTrue(oNewRole.ChangeTracker.IsMarkedForAddition);
            Assert.IsTrue(oCollection.ChangeTracker.IsDirty);
            Assert.IsTrue(oObject.ChangeTracker.HasDirtyObjectGraph);
        }

        [Test]
        public void ShouldDetectRemoveFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var person = _Fixture.Create<Person>();
            person.Roles.Add(_Fixture.Create<Employee>());
            person.Roles.Add(_Fixture.Create<Customer>());
            person.Roles.Add(_Fixture.Create<Supplier>());

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
            var oProp = oObject.Properties[propName];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var childObject = person.Roles.Last();
            var oChildObject = (ObjectObserver)observerCache.GetObserver(childObject, childObject.GetType());

            var result = removeCommand.Invoke(oCollection, oChildObject);

            // Assert:
            Assert.IsTrue(oChildObject.ChangeTracker.IsDirty);
            Assert.IsTrue(oChildObject.ChangeTracker.IsMarkedForRemoval);
            Assert.IsTrue(oCollection.ChangeTracker.IsDirty);
            Assert.IsTrue(oObject.ChangeTracker.HasDirtyObjectGraph);
        }

        [Test]
        public void ShouldLeaveCollectionUnchangedWhenAddingAndRemovingNewInstance()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var createCommand = container.Resolve<CreateObjectCommand>();
            var addCommand = container.Resolve<AddToCollectionCommand>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var person = _Fixture.Create<Person>();
            person.Roles.Add(_Fixture.Create<Employee>());
            person.Roles.Add(_Fixture.Create<Customer>());
            person.Roles.Add(_Fixture.Create<Supplier>());

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
            var oProp = oObject.Properties[propName];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var oNewItem = (ObjectObserver)createCommand.Invoke(typeof(Employee), null);
            ((Employee)oNewItem.RealObject).ID = Guid.NewGuid();

            var addResult = addCommand.Invoke(oCollection, oNewItem);
            var removeResult = removeCommand.Invoke(oCollection, oNewItem);

            // Assert:
            Assert.IsTrue(oNewItem.ChangeTracker.IsTransient);
            Assert.IsFalse(oNewItem.ChangeTracker.IsMarkedForAddition);
            Assert.IsFalse(oNewItem.ChangeTracker.IsMarkedForRemoval);
            Assert.IsFalse(oCollection.ChangeTracker.IsDirty);
            Assert.IsFalse(oObject.ChangeTracker.HasDirtyObjectGraph);
        }

        [Test]
        public void ShouldDetectChangesToLargeObjectGraph()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var person = _Fixture.Create<Person>();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var propName = LambdaExtensions.NameOf<Person>(x => x.Roles);
            var oProp = oObject.Properties[propName];

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
}