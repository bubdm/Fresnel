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

        public DbSet<Category> Categories { get; set; }
        public DbSet<DetailObject> DetailObjects { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Money> Money { get; set; }
        public DbSet<PocoObject> PocoObjects { get; set; }

        public T CreateObject<T>() where T : class
        {
            return _ObjectContext.CreateObject<T>();
        }

        public T GetObject<T>(Guid id) where T : class
        {
            var keyValues = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("ID", id)
            };
            var key = new EntityKey(this.CreateEntitySetName<T>(), keyValues);


            var result = (T)_ObjectContext.GetObjectByKey(key);
            return result;
        }

        private string CreateEntitySetName<T>()
        {
            return typeof(T).Name + "Set";
        }

        public void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class
        {
            _ObjectContext.LoadProperty<TParent>(parent, selector);
        }

        public void UpdateObject<T>(T entityWithChanges) where T : class
        {
            _ObjectContext.ApplyCurrentValues(this.CreateEntitySetName<T>(), entityWithChanges);
        }

        public void DeleteObject<T>(T entityToDelete) where T : class
        {
            _ObjectContext.DeleteObject(entityToDelete);
        }

    }
}
