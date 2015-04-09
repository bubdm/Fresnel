using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Fresnel.SampleModel.Persistence
{
    public class ModelContext : DbContext
    {
        private ModelConfigurator _Configurator;
        private ObjectContext _ObjectContext;
        private IDictionary<string, EntityType> _KnownTypes;

        public ModelContext(string nameOrConnectionString, ModelConfigurator configurator)
            : base(nameOrConnectionString)
        {
            _Configurator = configurator;
            _ObjectContext = ((IObjectContextAdapter)this).ObjectContext;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _Configurator.ExecuteOn(modelBuilder);
        }

        public DbSet<Address> AddressSet { get; set; }

        public DbSet<Category> CategorySet { get; set; }

        public DbSet<ContactDetails> ContactDetailsSet { get; set; }

        public DbSet<Country> CountrySet { get; set; }

        public DbSet<Customer> CustomerSet { get; set; }

        public DbSet<Employee> EmployeeSet { get; set; }

        public DbSet<Note> NoteSet { get; set; }

        public DbSet<Order> OrderSet { get; set; }

        public DbSet<OrderItem> OrderItemSet { get; set; }

        public DbSet<Organisation> OrganisationSet { get; set; }

        public DbSet<Person> PersonSet { get; set; }

        public DbSet<Product> ProductSet { get; set; }

        public DbSet<Region> RegionSet { get; set; }

        public DbSet<Role> RoleSet { get; set; }

        public DbSet<Shipment> ShipmentSet { get; set; }

        public DbSet<Shipper> ShipperSet { get; set; }

        public DbSet<StockDetail> StockDetailSet { get; set; }

        public DbSet<Supplier> SupplierSet { get; set; }

        public DbSet<Territory> TerritorySet { get; set; }

        public DbSet<BooleanValues> BooleanValuesSet { get; set; }

        public DbSet<MultiType> MultiTypeSet { get; set; }

        public DbSet<TextValues> TextValuesSet { get; set; }

        public bool IsKnownType(Type objectType)
        {
            if (_KnownTypes == null)
            {
                var items = _ObjectContext.MetadataWorkspace.GetItems<EntityType>(System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace);
                _KnownTypes = items.ToDictionary(i => i.Name);
            }

            EntityType match;
            _KnownTypes.TryGetValue(objectType.Name, out match);
            return match != null;
        }

        public object CreateObject(Type objectType)
        {
            var set = this.Set(objectType);

            var newObject = set.Create();
            set.Add(newObject);

            return newObject;
        }

        public object GetObject(Type objectType, Guid id)
        {
            var set = this.Set(objectType);
            var result = set.Find(id);
            return result;
        }

        public IQueryable GetObjects(Type objectType)
        {
            var set = this.Set(objectType);
            return set;
        }

        private string CreateEntitySetName<T>()
        {
            return this.CreateEntitySetName(typeof(T));
        }

        private string CreateEntitySetName(Type type)
        {
            // NB: This creates names that match the DbSet properties declared earlier:
            return string.Concat(this.GetType().Name, ".", type.Name, "Set");
        }

        public void LoadProperty(Type objectType, Guid id, string propertyName)
        {
            var set = this.Set(objectType);

            var entity = set.Find(id);

            var collectionEntry = this.Entry(entity).Collection(propertyName);
            if (collectionEntry != null)
            {
                collectionEntry.Load();
            }
            else
            {
                this.Entry(entity).Reference(propertyName).Load();
            }
        }

        public void Refresh(object entity)
        {
            _ObjectContext.Refresh(RefreshMode.StoreWins, entity);
        }

        public void UpdateObject(object entityWithChanges, Type objectType)
        {
            var entry = base.Entry(entityWithChanges);
            entry.State = EntityState.Modified;
        }

        public void DeleteObject(object entityToDelete, Type objectType)
        {
            _ObjectContext.DeleteObject(entityToDelete);
        }

        public int SaveChanges(params object[] entities)
        {
            // TODO: Make this only save the entities given, not the whole context:

            this.AttachMissingEntitiesToContext(entities);
            this.IncrementConcurrencyTokens();
            return base.SaveChanges();
        }

        private void AttachMissingEntitiesToContext(object[] entities)
        {
            foreach (var entity in entities)
            {
                var entityType = entity.GetType();
                if (!this.IsKnownType(entityType))
                    continue;

                var entry = this.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    // The context doesn't recognise it, so attach it:
                    var set = this.Set(entityType);
                    set.Add(entity);
                }
            }
        }

        private void IncrementConcurrencyTokens()
        {
            var changes = this.ChangeTracker
                            .Entries()
                            .Where(x => x.State == EntityState.Added ||
                                        x.State == EntityState.Modified);

            foreach (var entry in changes)
            {
                var entity = entry.Entity as IPersistable;
                if (entity != null)
                {
                    entity.Version++;
                }
            }
        }

        public T GetObject<T>(Guid id) where T : class
        {
            var keyValues = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("ID", id)
            };
            var entitySetName = this.CreateEntitySetName<T>();
            var key = new EntityKey(entitySetName, keyValues);

            var result = (T)_ObjectContext.GetObjectByKey(key);
            return result;
        }

        public IQueryable<T> GetObjects<T>()
            where T : class
        {
            return this.Set<T>();
        }

        public void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class
        {
            _ObjectContext.LoadProperty<TParent>(parent, selector);
        }

        public void UpdateObject<T>(T entityWithChanges) where T : class
        {
            var entitySetName = this.CreateEntitySetName<T>();
            _ObjectContext.ApplyCurrentValues(entitySetName, entityWithChanges);
        }

        public void DeleteObject<T>(T entityToDelete)
        {
            _ObjectContext.DeleteObject(entityToDelete);
        }
    }
}