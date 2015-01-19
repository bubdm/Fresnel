using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;

using NUnit.Framework;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ExplorerControllerTests
    {
        [Test()]
        public void ShouldReturnObjectProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            Assert.IsTrue(getResult.Passed);
            Assert.IsNotNull(getResult.ReturnValue);
        }

        [Test()]
        public void ShouldReturnCorrectHeadersForCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var observerCache = container.Resolve<ObserverCache>();
            var templateCache = container.Resolve<TemplateCache>();
            var controller = container.Resolve<ExplorerController>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();
            var oObject = observerCache.GetObserver(poco) as ObjectObserver;

            // Act:
            var request = new GetPropertyRequest()
            {
                ObjectID = poco.ID,
                PropertyName = "ChildObjects"
            };

            var getResult = controller.GetObjectProperty(request);

            // Assert:
            var collectionVM = (CollectionVM)getResult.ReturnValue;

            // There should be a Header for each viewable property in the collection's Element type:
            var tPoco = (ClassTemplate)templateCache.GetTemplate(poco.GetType());

            var visibleProperties = tPoco.Properties.Values.Where(p => !p.IsFrameworkMember && p.IsVisible);

            Assert.GreaterOrEqual(collectionVM.ElementProperties.Count(), visibleProperties.Count());
        }
    }
}