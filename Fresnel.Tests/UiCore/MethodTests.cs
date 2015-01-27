using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.TypeInfo;
using NUnit.Framework;

using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class MethodEditorTests
    {
        [Test()]
        public void ShouldIdentifyTextValues()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var vmBuilder = container.Resolve<AbstractObjectVMBuilder>();

            var obj = new SampleModel.MethodTests();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var vm = vmBuilder.BuildFor(oObject);

            // Assert:
            var methodWithParams = vm.Methods.First(m => m.Parameters.Count() > 1);

            Assert.IsTrue(methodWithParams.Parameters.Any(p => p.State.ValueType != null));
            Assert.IsTrue(methodWithParams.Parameters.Any(p => p.Info != null));
        }


    }
}