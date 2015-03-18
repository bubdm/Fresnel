﻿using Autofac;
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

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            // Act:
            var observer = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());

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

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());
            var oProp = oObject.Properties["NormalText"];
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
            var pocoType = typeof(SampleModel.Objects.PocoObject);
            var oObject = createCommand.Invoke(pocoType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            Assert.IsInstanceOf<SampleModel.Objects.PocoObject>(oObject.RealObject);
        }

        [Test]
        public void ShouldDetectAddToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());
            var oProp = oObject.Properties["ChildObjects"];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var newItem = new SampleModel.Objects.PocoObject();
            newItem.ID = Guid.NewGuid();
            var oNewItem = (ObjectObserver)observerCache.GetObserver(newItem, newItem.GetType());

            var result = addCommand.Invoke(oCollection, oNewItem);

            // Assert:
            Assert.IsTrue(oNewItem.ChangeTracker.IsDirty);
            Assert.IsTrue(oNewItem.ChangeTracker.IsMarkedForAddition);
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

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            poco.AddSomeChildObjects();

            var oObject = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());
            var oProp = oObject.Properties["ChildObjects"];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var childObject = poco.ChildObjects.Last();
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

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            poco.AddSomeChildObjects();

            var oObject = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());
            var oProp = oObject.Properties["ChildObjects"];
            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var oNewItem = (ObjectObserver)createCommand.Invoke(typeof(SampleModel.Objects.PocoObject), null);
            ((SampleModel.Objects.PocoObject)oNewItem.RealObject).ID = Guid.NewGuid();

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

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerCache.GetObserver(poco, poco.GetType());
            var oProp = oObject.Properties["ChildObjects"];

            var iterations = 10000;

            // Act:
            for (var i = 0; i < iterations; i++)
            {
                poco.ChildObjects.Add(new SampleModel.Objects.PocoObject());
            }

            var oCollection = (CollectionObserver)getCommand.Invoke(oProp);

            // Act:
            var oTextProp = oObject.Properties["NormalText"];
            var oValue = observerCache.GetObserver("1234", typeof(string));
            setCommand.Invoke(oTextProp, oValue);

            // Assert:
            oCollection = (CollectionObserver)getCommand.Invoke(oProp);
            Assert.AreEqual(iterations, oCollection.GetItems().Cast<object>().Count());
        }
    }
}