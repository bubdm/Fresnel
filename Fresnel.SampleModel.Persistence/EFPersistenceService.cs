using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fresnel.SampleModel.Persistence
{
    public class EFPersistenceService : IPersistenceService
    {
        private Func<ModelContext> _ModelContextFactory;
        private ModelContext _ModelContext;

        public EFPersistenceService(Func<ModelContext> modelContextFactory)
        {
            _ModelContextFactory = modelContextFactory;
            _ModelContext = _ModelContextFactory();
        }

        public bool IsTypeRecognised(Type objectType)
        {
            return _ModelContext.IsKnownType(objectType);
        }

        public object GetObject(Type objectType, Guid id)
        {
            return _ModelContext.GetObject(objectType, id);
        }

        public IQueryable GetObjects(Type objectType)
        {
            return _ModelContext.GetObjects(objectType);
        }

        public void LoadProperty(Type objectType, Guid id, string propertyName)
        {
            _ModelContext.LoadProperty(objectType, id, propertyName);
        }

        public void Refresh(object entity)
        {
            _ModelContext.Refresh(entity);
        }

        public void UpdateObject(object entityWithChanges, Type objectType)
        {
            _ModelContext.UpdateObject(entityWithChanges, objectType);
        }

        public void DeleteObject(object entityToDelete, Type objectType)
        {
            _ModelContext.DeleteObject(entityToDelete, objectType);
        }

        public int SaveChanges(params object[] entities)
        {
            return _ModelContext.SaveChanges(entities);
        }

        public void RollbackChanges()
        {
            // See http://stackoverflow.com/a/5468570/80369
            _ModelContext.Dispose();
            _ModelContext = _ModelContextFactory();
        }

        public T GetObject<T>(Guid id) where T : class
        {
            return _ModelContext.GetObject<T>(id);
        }

        public IQueryable<T> GetObjects<T>() where T : class
        {
            return _ModelContext.GetObjects<T>();
        }

        public void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector) where TParent : class
        {
            _ModelContext.LoadProperty<TParent>(parent, selector);
        }

        public void UpdateObject<T>(T entityWithChanges) where T : class
        {
            _ModelContext.UpdateObject<T>(entityWithChanges);
        }

        public void DeleteObject<T>(T entityToDelete) where T : class
        {
            _ModelContext.DeleteObject<T>(entityToDelete);
        }
    }
}