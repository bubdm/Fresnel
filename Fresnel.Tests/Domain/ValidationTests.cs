using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using NUnit.Framework;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ValidationTests
    {
        [Test]
        public void ShouldNotSetInvalidString()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var obj = new SampleModel.TestTypes.TextValues();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);

            var oProp = oObj.Properties["TextWithSize"];

            var oInvalidValue = observerCache.GetObserver("1234");

            // Act:
            var exception = Assert.Throws<AggregateException>(() => setCommand.Invoke(oProp, oInvalidValue));
            var minLengthException = exception.InnerExceptions.First();
            Assert.IsTrue(minLengthException.Message.Contains("8 char"));
        }

        [Test]
        public void ShouldNotSetInvalidNumber()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var obj = new SampleModel.TestTypes.NumberValues();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);

            var oProp = oObj.Properties["NumberWithRange"];

            var oInvalidValue = observerCache.GetObserver(9999);

            // Act:
            var exception = Assert.Throws<AggregateException>(() => setCommand.Invoke(oProp, oInvalidValue));
            var minLengthException = exception.InnerExceptions.First();
            Assert.IsTrue(minLengthException.Message.Contains("-234"));
        }

    }
}