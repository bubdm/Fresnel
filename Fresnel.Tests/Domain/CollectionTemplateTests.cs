using Autofac;
using NUnit.Framework;
using System;
using System.Linq;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionTemplateTests
    {

        [Test()]
        public void ShouldAddToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();
            Assert.AreEqual(0, pocoObject.ChildObjects.Count);

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];
            var collectionTemplate = (CollectionTemplate)collectionPropertyTemplate.InnerClass;

            // Act:
            var newItem = new SampleModel.Objects.PocoObject();
            addCommand.Invoke(collectionTemplate, pocoObject.ChildObjects, classTemplate, newItem);

            // Assert:
            Assert.AreNotEqual(0, pocoObject.ChildObjects.Count);
        }

        [Test()]
        public void ShouldAddToCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();
            Assert.AreEqual(0, pocoObject.ChildObjects.Count);

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];

            // Act:
            var newItem = new SampleModel.Objects.PocoObject();
            addCommand.Invoke(pocoObject, collectionPropertyTemplate, newItem);

            // Assert:
            Assert.AreNotEqual(0, pocoObject.ChildObjects.Count);
        }

        [Test()]
        public void ShouldRemoveFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();
            pocoObject.AddSomeChildObjects();
            Assert.AreNotEqual(0, pocoObject.ChildObjects.Count);

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];
            var collectionTemplate = (CollectionTemplate)collectionPropertyTemplate.InnerClass;

            // Act:
            var items = pocoObject.ChildObjects.ToList();
            foreach (var item in items)
            {
                removeCommand.Invoke(collectionTemplate, pocoObject.ChildObjects, classTemplate, item);
            }

            // Assert:
            Assert.AreEqual(0, pocoObject.ChildObjects.Count);
        }

        [Test()]
        public void ShouldRemoveFromCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var pocoObject = new SampleModel.Objects.PocoObject();
            Assert.AreEqual(0, pocoObject.ChildObjects.Count);

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(pocoObject.GetType());
            var collectionPropertyTemplate = classTemplate.Properties["ChildObjects"];

            // Act:
            var items = pocoObject.ChildObjects.ToList();
            foreach (var item in items)
            {
                removeCommand.Invoke(pocoObject, collectionPropertyTemplate, item);
            }
            // Assert:
            Assert.AreEqual(0, pocoObject.ChildObjects.Count);
        }

    }
}

