//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using NUnit.Framework;
using Autofac;
using System;
using System.Linq;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;
using System.Collections.Generic;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Commands;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ChangeTrackingTests
    {
        [Test()]
        public void ShouldHaveDefaultChangeStatus()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            // Act:
            var observer = (ObjectObserver)observerBuilder.BuildFor(poco, poco.GetType());

            // Assert:
            Assert.IsFalse(observer.ChangeTracker.IsDirty);
        }

        [Test()]
        public void ShouldDetectPropertyChanges()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            var oObject = (ObjectObserver)observerBuilder.BuildFor(poco, poco.GetType());
            var oProp = oObject.Properties["NormalText"];
            var oValue = observerBuilder.BuildFor("1234", typeof(string));

            // Act:
            setCommand.Invoke(oProp, oValue);

            // Assert:
            Assert.IsTrue(oObject.ChangeTracker.IsDirty);
        }

        [Test()]
        public void ShouldDetectNewInstances()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var createaCommand = container.Resolve<CreateObjectCommand>();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            // Act:
            var pocoType = typeof(SampleModel.Objects.PocoObject);
            var oObject = createaCommand.Invoke(pocoType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            Assert.IsInstanceOf<SampleModel.Objects.PocoObject>(oObject.RealObject);
        }


    }


}

