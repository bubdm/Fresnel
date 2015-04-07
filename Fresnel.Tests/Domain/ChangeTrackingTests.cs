using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using NUnit.Framework;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ChangeTrackingTests
    {
        [Test]
        public void ShouldHaveDefaultChangeStatus()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

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

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var oProp = oObject.Properties["FirstName"];
            var oValue = observerCache.GetObserver("1234", typeof(string));

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
            var personType = typeof(SampleModel.Northwind.Person);
            var oObject = createCommand.Invoke(personType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            Assert.IsInstanceOf<SampleModel.Northwind.Person>(oObject.RealObject);
        }

        [Test]
        public void ShouldDetectAddToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var oProp = oObject.Properties["Roles"];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var newRole = new SampleModel.Northwind.Employee();
            newRole.ID = Guid.NewGuid();
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

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

            person.Roles.Add(new SampleModel.Northwind.Employee());
            person.Roles.Add(new SampleModel.Northwind.Customer());
            person.Roles.Add(new SampleModel.Northwind.Supplier());

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var oProp = oObject.Properties["Roles"];
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

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

            person.Roles.Add(new SampleModel.Northwind.Employee());
            person.Roles.Add(new SampleModel.Northwind.Customer());
            person.Roles.Add(new SampleModel.Northwind.Supplier());

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var oProp = oObject.Properties["Roles"];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var oNewItem = (ObjectObserver)createCommand.Invoke(typeof(SampleModel.Northwind.Employee), null);
            ((SampleModel.Northwind.Employee)oNewItem.RealObject).ID = Guid.NewGuid();

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

            var person = new SampleModel.Northwind.Person();
            person.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(person, person.GetType());
            var oProp = oObject.Properties["Roles"];

            var iterations = 10000;

            // Act:
            for (var i = 0; i < iterations; i++)
            {
                person.Roles.Add(new SampleModel.Northwind.Employee());
            }

            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var oNameProp = oObject.Properties["FirstName"];
            var oValue = observerCache.GetObserver("1234", typeof(string));
            setCommand.Invoke(oNameProp, oValue);

            // Assert:
            oCollection = (CollectionObserver)getCommand.Invoke(oProp);
            Assert.AreEqual(iterations, oCollection.GetItems().Cast<object>().Count());
        }
    }
}