//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using Autofac;
using NUnit.Framework;
using System;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionTests
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
        public void ShouldRemoveFromCollection()
        {
            Assert.Inconclusive();
        }

    }
}

