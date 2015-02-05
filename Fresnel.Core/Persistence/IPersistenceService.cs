﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Envivo.Fresnel.Core.Persistence
{
    public interface IPersistenceService
    {                
        object CreateObject(Type objectType);

        object GetObject(Type objectType, Guid id);

        IQueryable GetObjects(Type objectType);

        void LoadProperty(Type objectType, Guid id, string propertyName);

        void Refresh(object entity);

        void UpdateObject(object entityWithChanges, Type objectType);

        void DeleteObject(object entityWithChanges, Type objectType);

        int SaveChanges();

        T CreateObject<T>()
            where T : class;

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