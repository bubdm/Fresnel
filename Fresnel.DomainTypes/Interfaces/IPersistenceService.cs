using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    public interface IPersistenceService : IDomainDependency, IDisposable
    {
        bool IsTypeRecognised(Type objectType);

        object CreateObject(Type objectType, object constructorArg);

        object GetObject(Type objectType, Guid id, string[] propertiesToInclude);

        IQueryable GetObjects(Type objectType, string[] propertiesToInclude);

        void LoadProperty(object entity, string propertyName);

        void Refresh(object entity);

        void UpdateObject(object entityWithChanges, Type objectType);

        void DeleteObject(object entityWithChanges, Type objectType);

        int SaveChanges(IEnumerable<object> newEntities, IEnumerable<object> modifiedEntities);

        void RollbackChanges();

        T GetObject<T>(Guid id)
            where T : class;

        IQueryable<T> GetObjects<T>()
            where T : class;

        void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class;

        void UpdateObject<T>(T entityWithChanges)
            where T : class;

        void DeleteObject<T>(T entityToDelete)
            where T : class;
    }
}