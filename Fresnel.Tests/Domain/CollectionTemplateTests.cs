using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionTemplateTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldAddToCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var category = _Fixture.Create<Category>();
            Assert.AreEqual(0, category.Products.Count);

            var tCategory = (ClassTemplate)templateCache.GetTemplate(category.GetType());
            var propertyName = LambdaExtensions.NameOf<Category>(x => x.Products);
            var tProperty = tCategory.Properties[propertyName];
            var tCollection = (CollectionTemplate)tProperty.InnerClass;

            // Act:
            var product = _Fixture.Create<Product>();
            var tProduct = (ClassTemplate)templateCache.GetTemplate(product.GetType());
            addCommand.Invoke(tCollection, category.Products, tProduct, product);

            // Assert:
            Assert.AreNotEqual(0, category.Products.Count);
        }

        [Test]
        public void ShouldAddToCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var addCommand = container.Resolve<AddToCollectionCommand>();

            var category = _Fixture.Create<Category>();
            Assert.AreEqual(0, category.Products.Count);

            var tCategory = (ClassTemplate)templateCache.GetTemplate(category.GetType());
            var propertyName = LambdaExtensions.NameOf<Category>(x => x.Products);
            var tProperty = tCategory.Properties[propertyName];

            // Act:
            var product = _Fixture.Create<Product>();
            addCommand.Invoke(category, tProperty, product);

            // Assert:
            Assert.AreNotEqual(0, category.Products.Count);
        }

        [Test]
        public void ShouldRemoveFromCollection()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var category = _Fixture.Create<Category>();
            category.Products.AddMany(() => _Fixture.Create<Product>(), 5);
            Assert.AreNotEqual(0, category.Products.Count);

            var tCategory = (ClassTemplate)templateCache.GetTemplate(category.GetType());
            var propertyName = LambdaExtensions.NameOf<Category>(x => x.Products);
            var tProperty = tCategory.Properties[propertyName];
            var tCollection = (CollectionTemplate)tProperty.InnerClass;

            // Act:
            var products = category.Products.ToList();
            foreach (var product in products)
            {
                var tProduct = (ClassTemplate)templateCache.GetTemplate(product.GetType());
                removeCommand.Invoke(tCollection, category.Products, tProduct, product);
            }

            // Assert:
            Assert.AreEqual(0, category.Products.Count);
        }

        [Test]
        public void ShouldRemoveFromCollectionProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var removeCommand = container.Resolve<RemoveFromCollectionCommand>();

            var category = _Fixture.Create<Category>();

            Assert.AreEqual(0, category.Products.Count);

            var tCategory = (ClassTemplate)templateCache.GetTemplate(category.GetType());
            var propertyName = LambdaExtensions.NameOf<Category>(x => x.Products);
            var tProperty = tCategory.Properties[propertyName];

            // Act:
            var products = category.Products.ToList();
            foreach (var product in products)
            {
                removeCommand.Invoke(category, tProperty, product);
            }
            // Assert:
            Assert.AreEqual(0, category.Products.Count);
        }
    }
}