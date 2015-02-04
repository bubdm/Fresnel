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

namespace Fresnel.SampleModel.Persistence
{
    public class ModelContext : DbContext
    {
        private ObjectContext _ObjectContext;

        public ModelContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            _ObjectContext = ((IObjectContextAdapter)this).ObjectContext;
        }

        public DbSet<Category> CategorySet { get; set; }
        public DbSet<DetailObject> DetailObjectSet { get; set; }
        public DbSet<Product> ProductSet { get; set; }
        public DbSet<Money> MoneySet { get; set; }
        public DbSet<PocoObject> PocoObjectSet { get; set; }

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

        private string CreateEntitySetName<T>()
        {
            // NB: This creates names that match the DbSet properties declared earlier:
            return string.Concat(this.GetType().Name, ".", typeof(T).Name, "Set");
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

        public void DeleteObject<T>(T entityToDelete) where T : class
        {
            _ObjectContext.DeleteObject(entityToDelete);
        }

    }
}
