using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ValidationTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldNotSetInvalidString()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var obj = _Fixture.Create<TextValues>();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);

            var propName = LambdaExtensions.NameOf<TextValues>(x => x.TextWithSize);
            var oProp = oObj.Properties[propName];

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

            var obj = _Fixture.Create<NumberValues>();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);

            var propName = LambdaExtensions.NameOf<NumberValues>(x => x.NumberWithRange);
            var oProp = oObj.Properties[propName];

            var oInvalidValue = observerCache.GetObserver(9999);

            // Act:
            var exception = Assert.Throws<AggregateException>(() => setCommand.Invoke(oProp, oInvalidValue));
            var minLengthException = exception.InnerExceptions.First();
            Assert.IsTrue(minLengthException.Message.Contains("-234"));
        }

    }
}