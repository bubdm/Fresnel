using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class PropertyTemplateTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldGetProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();

            var product = _Fixture.Create<Product>();

            var template = (ClassTemplate)templateCache.GetTemplate(product.GetType());

            // Act:
            var propName = LambdaExtensions.NameOf<Product>(x => x.Name);
            var value = getCommand.Invoke(template, product, propName);

            // Assert:
            Assert.AreEqual(product.Name, value);
        }

        [Test]
        public void ShouldSetProperty()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var setCommand = container.Resolve<SetPropertyCommand>();

            var product = _Fixture.Create<Product>();

            var template = (ClassTemplate)templateCache.GetTemplate(product.GetType());

            // Act:
            var newValue = DateTime.Now.ToString();
            var propName = LambdaExtensions.NameOf<Product>(x => x.Name);
            setCommand.Invoke(template, product, propName, newValue);

            // Assert:
            Assert.AreEqual(newValue, product.Name);
        }

        [Test]
        public void ShouldGetBackingField()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var getCommand = container.Resolve<GetBackingFieldCommand>();

            var product = _Fixture.Create<Product>();

            var template = (ClassTemplate)templateCache.GetTemplate(product.GetType());

            // Act:
            var propName = LambdaExtensions.NameOf<Product>(x => x.Name);
            var value = getCommand.Invoke(template, product, propName);

            // Assert:
            Assert.AreEqual(product.Name, value);
        }

        [Test]
        public void ShouldSetBackingField()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var setCommand = container.Resolve<SetBackingFieldCommand>();

            var product = _Fixture.Create<Product>();

            var template = (ClassTemplate)templateCache.GetTemplate(product.GetType());

            // Act:
            var newValue = DateTime.Now.ToString();
            var propName = LambdaExtensions.NameOf<Product>(x => x.Name);
            setCommand.Invoke(template, product, propName, newValue);

            // Assert:
            Assert.AreEqual(newValue, product.Name);
        }

        [Test]
        public void ShouldIdentifyInheritedProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var getCommand = container.Resolve<GetPropertyCommand>();

            var employee = _Fixture.Create<Employee>();

            var template = (ClassTemplate)templateCache.GetTemplate(employee.GetType());

            // Act:
            var personPropName = LambdaExtensions.NameOf<Employee>(x => x.Person);
            var person = getCommand.Invoke(template, employee, personPropName);

            var hiredOnPropName = LambdaExtensions.NameOf<Employee>(x => x.HiredOn);
            var hiredOn = getCommand.Invoke(template, employee, hiredOnPropName);

            // Assert:
            Assert.AreEqual(employee.Person, person);
            Assert.AreEqual(employee.HiredOn, hiredOn);
        }

        [Test]
        public void ShouldIdentifyCollectionPropertyAttributes()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();

            var order = _Fixture.Create<Order>();

            var template = (ClassTemplate)templateCache.GetTemplate(order.GetType());

            // Act:
            var propertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems);
            var tProperty = template.Properties.Values.Single(p => p.Name == propertyName);

            // Assert:
            Assert.IsFalse(tProperty.CanAdd);
            Assert.IsTrue(tProperty.CanCreate);
            Assert.IsTrue(tProperty.CanRead);
            Assert.IsTrue(tProperty.CanWrite);

            Assert.IsTrue(tProperty.IsCompositeRelationship);
            Assert.IsFalse(tProperty.IsAggregateRelationship);
        }
    }
}