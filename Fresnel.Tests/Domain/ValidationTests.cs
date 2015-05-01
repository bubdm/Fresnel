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
        public void ShouldNotSetInvalidString()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var setCommand = _TestScopeContainer.Resolve<SetPropertyCommand>();

                observerCache.CleanUp();
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
        }

        [Test]
        public void ShouldNotSetInvalidNumber()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerCache = _TestScopeContainer.Resolve<ObserverCache>();
                var setCommand = _TestScopeContainer.Resolve<SetPropertyCommand>();

                observerCache.CleanUp();
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
}