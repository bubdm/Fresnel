using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Envivo.Fresnel.Core.Persistence;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Fresnel.SampleModel.Persistence
{
    public class ModelContext : DbContext
    {
        private ModelConfigurator _Configurator;
        private ObjectContext _ObjectContext;

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

        public DbSet<Category> CategorySet { get; set; }
        public DbSet<MasterObject> MasterObjectSet { get; set; }
        public DbSet<DetailObject> DetailObjectSet { get; set; }
        public DbSet<Product> ProductSet { get; set; }
        public DbSet<Money> MoneySet { get; set; }
        public DbSet<PocoObject> PocoObjectSet { get; set; }

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
            this.IncrementConcurrencyTokens();
            return base.SaveChanges();
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

        public T CreateObject<T>() where T : class
        {
            var newObject = _ObjectContext.CreateObject<T>();
            this.Set<T>().Add(newObject);

            return newObject;
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
