using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ObjectObserverTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldCreateObjectObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var employee = _Fixture.Create<Employee>();

            // Act:
            var observer = observerBuilder.BuildFor(employee, employee.GetType());

            // Assert:
            Assert.IsNotNull(observer);
        }

        [Test]
        public void ShouldCreateNullObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            // Act:
            var observer = observerBuilder.BuildFor(null, typeof(Employee));

            // Assert:
            Assert.IsInstanceOf<NullObserver>(observer);
            Assert.IsNull(observer.RealObject);
            Assert.AreEqual(typeof(Employee), observer.Template.RealType);
        }

        [Test]
        public void ShouldGetObjectObserverFromCache()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employee = _Fixture.Create<Employee>();

            // Act:
            var observer = observerCache.GetObserver(employee);

            // Assert:
            Assert.IsNotNull(observer);
        }

        [Test]
        public void ShouldReplaceInvalidIdForTrackableObjects()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employee = _Fixture.Create<Employee>();
            
            // Act:
            var observer = observerCache.GetObserver(employee);

            // Assert:
            Assert.AreNotEqual(Guid.Empty, employee.ID);
            Assert.AreEqual(employee.ID, observer.ID);
        }

        [Test]
        public void ShouldLocateTrackableObjectsByID()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employee = _Fixture.Create<Employee>();
            
            // Act:
            var oObject = observerCache.GetObserver(employee);
            var objFromCache = observerCache.GetObserverById(oObject.ID).RealObject;

            // Assert:
            Assert.AreEqual(employee, objFromCache);
        }

        [Test]
        public void ShouldLocateNonTrackableObjectsByID()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employees = _Fixture.CreateMany<Employee>().ToList();
            
            // Act:
            var oCollection = observerCache.GetObserver(employees);
            var objFromCache = observerCache.GetObserverById(oCollection.ID).RealObject;

            // Assert:
            Assert.AreEqual(employees, objFromCache);
        }

        [Test]
        public void ShouldGetSameCachedObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employee = _Fixture.Create<Employee>();

            // Act:
            var observer1 = observerCache.GetObserver(employee);
            var observer2 = observerCache.GetObserver(employee);

            // Assert:
            Assert.AreSame(observer1, observer2);
        }

        [Test]
        public void ShouldReturnSeparateObserversForSeparateInstances()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            var employee1 = _Fixture.Create<Employee>();
            var employee2 = _Fixture.Create<Employee>();

            // Act:
            var observer1 = observerCache.GetObserver(employee1);
            var observer2 = observerCache.GetObserver(employee2);

            // Assert:
            Assert.AreNotSame(observer1, observer2);
        }

        [Test]
        public void ShouldCreatePropertyObservers()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var employee = _Fixture.Create<Employee>();

            // Act:
            var observer = (ObjectObserver)observerBuilder.BuildFor(employee, employee.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Properties.Count());

            foreach (var prop in observer.Properties.Values)
            {
                Assert.IsNotNull(prop.FullName);
            }
        }

        [Test]
        public void ShouldCreateMethodObservers()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var employee = _Fixture.Create<Employee>();

            // Act:
            var observer = (ObjectObserver)observerBuilder.BuildFor(employee, employee.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Methods.Count());

            foreach (var method in observer.Methods.Values)
            {
                var parameters = method.Parameters;
                Assert.IsNotNull(method.Parameters);

                foreach (var p in parameters.Values)
                {
                    Assert.IsNotNull(p.FullName);
                }
            }
        }

        [Test]
        public void ShouldCreateCollectionObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var employee = _Fixture.Create<Employee>();
            employee.Notes.AddMany(() => _Fixture.Create<Note>(), 5);

            // Act:
            var observer = observerBuilder.BuildFor(employee.Notes, employee.Notes.GetType());

            // Assert:
            Assert.IsNotNull(observer);
        }

        [Test]
        public void ShouldCleanupCache()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();

            for (var i = 0; i < 5; i++)
            {
                var employee = _Fixture.Create<Employee>();
                employee.Notes.AddMany(() => _Fixture.Create<Note>(), 3);

                var observer = observerCache.GetObserver(employee);
            }

            // Act:
            observerCache.CleanUp();

            // Assert:
            Assert.AreEqual(0, observerCache.GetAllObservers().Count());
        }
    }
}