using System;
using System.Linq.Expressions;
namespace Envivo.Fresnel.Core.Persistence
{
    public interface IPersistenceService
    {
        T CreateObject<T>()
            where T : class;

        T GetObject<T>(Guid id)
            where T : class;

        void LoadProperty<TParent>(TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class;

        void UpdateObject<T>(T entityWithChanges)
            where T : class;

        void DeleteObject<T>(T entityToDelete)
            where T : class;


        int SaveChanges();

    }
}