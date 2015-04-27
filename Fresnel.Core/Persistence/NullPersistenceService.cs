using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Envivo.Fresnel.Core.Persistence
{
    public class NullPersistenceService: IPersistenceService
    {
        private IQueryable _DummyList = new List<IEntity>().AsQueryable();

        public bool IsTypeRecognised(Type objectType)
        {
            return true;
        }

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
            return _DummyList;
        }

        public void LoadProperty(object entity, string propertyName)
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

        public int SaveChanges(IEnumerable<object> newEntities, IEnumerable<object> modifiedEntities)
        {
            return -1;
        }

        public void RollbackChanges()
        {
        
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
        
        public void Dispose()
        {
        
        }
    }
}