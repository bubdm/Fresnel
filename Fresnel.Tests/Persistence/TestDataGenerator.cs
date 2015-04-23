using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using Autofac;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;

namespace Envivo.Fresnel.Tests.Persistence
{
    public class TestDataGenerator
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private int _ItemCount = 50;

        public void Generate()
        {
            var persistenceService = this.GetPersistenceService();

            this.Create<MultiType>(persistenceService);
            this.Create<TextValues>(persistenceService);
            this.Create<BooleanValues>(persistenceService);

            this.Create<Person>(persistenceService);
            this.Create<Organisation>(persistenceService);

            // TODO: Randomly tie Roles back to some Parties:
            this.Create<Customer>(persistenceService);
            this.Create<Employee>(persistenceService);
            this.Create<Supplier>(persistenceService);
            this.Create<Shipper>(persistenceService);

            this.CreateCategories(persistenceService);
        }

        private IPersistenceService GetPersistenceService()
        {
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();
            return persistenceService;
        }

        private void Create<T>(IPersistenceService persistenceService)
            where T : class
        {
            if (persistenceService.GetObjects<T>().Any())
                return;

            var entities = new List<T>();

            for (var i = 0; i < _ItemCount; i++)
            {
                var entity = _Fixture.Create<T>();
                entities.Add(entity);
            }

            persistenceService.SaveChanges(entities.ToArray());
        }

        private void CreateCategories(IPersistenceService persistenceService)
        {
            if (persistenceService.GetObjects<Category>().Any())
                return;

            var entities = new List<Category>();

            for (var i = 1; i < _ItemCount; i++)
            {
                var entity = _Fixture.Create<Category>();
                entity.Products.AddMany(() => _Fixture.Create<Product>(), 3);
                entities.Add(entity);
            }

            persistenceService.SaveChanges(entities.ToArray());
        }


    }
}

