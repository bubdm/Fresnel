using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Envivo.Fresnel.Core.Persistence
{
    public class NullPersistenceService: IPersistenceService
    {

        public object CreateObject(Type objectType)
        {
            return null;
        }

        public object GetObject(Type objectType, Guid id)
        {
            return null;
        }

        public IQueryable GetObjects(Type objectType)
        {
            return null;
        }

        public void LoadProperty(Type objectType, Guid id, string propertyName)
        {
            
        }

        public void Refresh(object entity)
        {
            
        }

        public void UpdateObject(object entityWithChanges, Type objectType)
        {
            
        }

        public void DeleteObject(object entityWithChanges, Type objectType)
        {
            
        }

        public int SaveChanges()
        {
            return -1;
        }

        public T CreateObject<T>() where T : class
        {
            return null;
        }

        public T GetObject<T>(Guid id) where T : class
        {
            return null;
        }

        public IQueryable<T> GetObjects<T>() where T : class
        {
            return null;
        }

        public void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector) where TParent : class
        {
            
        }

        public void UpdateObject<T>(T entityWithChanges) where T : class
        {
            
        }

        public void DeleteObject<T>(T entityToDelete) where T : class
        {
            
        }
    }
}