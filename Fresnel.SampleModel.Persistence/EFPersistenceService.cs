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

namespace Fresnel.SampleModel.Persistence
{
    public class EFPersistenceService : IPersistenceService
    {
        private ModelContext _ModelContext;

        public EFPersistenceService(ModelContext modelContext)
        {
            _ModelContext = modelContext;
        }

        public T CreateObject<T>() where T : class
        {
            return _ModelContext.CreateObject<T>();
        }

        public T GetObject<T>(Guid id) where T : class
        {
            return _ModelContext.GetObject<T>(id);
        }

        public IQueryable<T> GetAll<T>()
            where T : class
        {
            return _ModelContext.GetAll<T>();
        }

        public void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class
        {
            _ModelContext.LoadProperty(parent, selector);
        }

        public void Refresh<T>(T entity) where T : class
        {
            _ModelContext.Refresh<T>(entity);
        }

        public void UpdateObject<T>(T entityWithChanges) where T : class
        {
            _ModelContext.UpdateObject(entityWithChanges);
        }

        public void DeleteObject<T>(T entityToDelete) where T : class
        {
            _ModelContext.DeleteObject(entityToDelete);
        }

        public int SaveChanges()
        {
            return _ModelContext.SaveChanges();
        }

    }
}
