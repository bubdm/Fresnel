using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.SampleModel.Northwind.Places;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections;
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
    public class ModelContext : DbContext, IDisposable
    {
        private ModelConfigurator _Configurator;
        private ObjectContext _ObjectContext;
        private RealTypeResolver _RealTypeResolver;
        private IDictionary<string, EntityType> _KnownTypes;

        public ModelContext
            (
            string nameOrConnectionString,
            ModelConfigurator configurator,
            RealTypeResolver realTypeResolver
            )
            : base(nameOrConnectionString)
        {
            _Configurator = configurator;
            _ObjectContext = ((IObjectContextAdapter)this).ObjectContext;
            _RealTypeResolver = realTypeResolver;

            _ObjectContext.ContextOptions.LazyLoadingEnabled = false;

#if DEBUG
            this.Database.Log = Console.Write;
            Console.Write("Created new ModelContext");
#endif
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

        public object GetObject(Type objectType, Guid id, string[] propertiesToInclude)
        {
            var set = this.Set(objectType);
            var result = set.Find(id);

            if (result != null)
            {
                foreach (var propName in propertiesToInclude)
                {
                    this.LoadProperty(result, propName);
                }
            }

            return result;
        }

        public IQueryable GetObjects(Type objectType, string[] propertiesToInclude)
        {
            DbQuery set = this.Set(objectType);
            foreach (var propName in propertiesToInclude)
            {
                set = set.Include(propName);
            }
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

        public void LoadProperty(object entity, string propertyName)
        {
            this.AttachPersistentEntityToContext(entity);
            var entry = this.Entry(entity);

            var memberEntry = entry.Member(propertyName);
            var collectionEntry = memberEntry as DbCollectionEntry;
            var referenceEntry = memberEntry as DbReferenceEntry;
            if (collectionEntry != null)
            {
                // Force the property to load, even if it has been loaded already:
                collectionEntry.IsLoaded = false;
                collectionEntry.Load();
            }
            else if (referenceEntry != null)
            {
                // Force the property to load, even if it has been loaded already:
                referenceEntry.IsLoaded = false;
                referenceEntry.Load();
            }
        }

        public void Refresh(object entity)
        {
            this.AttachPersistentEntityToContext(entity);
            _ObjectContext.Refresh(RefreshMode.StoreWins, entity);
        }

        public void UpdateObject(object entityWithChanges, Type objectType)
        {
            var entry = this.Entry(entityWithChanges);
            entry.State = EntityState.Modified;
        }

        public void DeleteObject(object entityToDelete, Type objectType)
        {
            _ObjectContext.DeleteObject(entityToDelete);
        }

        public int SaveChanges(IEnumerable<object> newEntities, IEnumerable<object> modifiedEntities)
        {
            // TODO: Make this only save the entities given, not the whole context:

            this.AddTransientEntitiesToContext(newEntities);
            this.AttachPersistentEntitiesToContext(modifiedEntities);

            return base.SaveChanges();
        }

        private void AddTransientEntitiesToContext(IEnumerable<object> transientEntities)
        {
            foreach (var entity in transientEntities)
            {
                this.AddTransientEntityToContext(entity);
            }
        }

        private void AddTransientEntityToContext(object transientEntity)
        {
            var entityType = _RealTypeResolver.GetRealType(transientEntity) ?? transientEntity.GetType();
            if (!this.IsKnownType(entityType))
                return;

            var entry = this.Entry(transientEntity);
            if (entry.State == EntityState.Detached)
            {
                var set = this.Set(entityType);
                set.Add(transientEntity);
            }
        }

        private void AttachPersistentEntitiesToContext(IEnumerable<object> persistentEntities)
        {
            foreach (var entity in persistentEntities)
            {
                this.AttachPersistentEntityToContext(entity);
            }
        }

        private void AttachPersistentEntityToContext(object persistentEntity)
        {
            var entityType = _RealTypeResolver.GetRealType(persistentEntity) ?? persistentEntity.GetType();
            if (!this.IsKnownType(entityType))
                return;

            var entry = this.Entry(persistentEntity);
            if (entry.State == EntityState.Detached)
            {
                var set = this.Set(entityType);
                set.Attach(persistentEntity);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ObjectContext.ContextOptions.LazyLoadingEnabled)
                {
                    AllowEntitiesToBeUsedInOtherContexts();
                }
            }

            base.Dispose(disposing);
        }

        private void AllowEntitiesToBeUsedInOtherContexts()
        {
            var entityStates = EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged;
            var entries = _ObjectContext.ObjectStateManager.GetObjectStateEntries(entityStates)
                               .Where(e => e.Entity != null)
                               .Where(e => e.State != EntityState.Detached)
                               .Where(e => !e.IsRelationship)
                               .ToArray();

            foreach (var entry in entries)
            {
                entry.ChangeState(EntityState.Detached);
            }
        }

    }
}