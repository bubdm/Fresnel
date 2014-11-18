using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Introspection.Configuration;

namespace Envivo.Fresnel.Introspection
{

    public static class TypeExtensions
    {
        private static IsObjectTrackableSpecification s_IsObjectTrackableSpecification = new IsObjectTrackableSpecification();
        private static IsObjectAuditableSpecification s_IsObjectAuditableSpecification = new IsObjectAuditableSpecification();
        private static IsObjectValidatableSpecification s_IsObjectValidatableSpecification = new IsObjectValidatableSpecification();

        static private Type IEntityType = typeof(IEntity);
        static private Type IValueObjectType = typeof(IValueObject);
        static private Type IAggregateRootType = typeof(IAggregateRoot);
        static private Type IListAdapterType = typeof(IListAdapter<>);

        static private Type IFactoryType = typeof(IFactory<>);
        static private Type IRepositoryType = typeof(IRepository<>);
        static private Type IDomainServiceType = typeof(IDomainService);
        //static private Type IPresenterType = typeof(UI.IPresenterInfrastructureService<>);
        static private Type IDataErrorInfoType = typeof(IDataErrorInfo);

        static internal Type IGenericDictionary = typeof(IDictionary<,>);
        static internal Type IGenericCollection = typeof(ICollection<>);
        static internal Type IGenericEnumerable = typeof(IEnumerable<>);

        /// <summary>
        /// Determines if the given type implements IEntity
        /// </summary>
        /// <param name="type"></param>
        
        /// <remarks>The value is determined if the Object implements the IEntity interface</remarks>
        public static bool IsEntity(this Type type)
        {
            return type.IsDerivedFrom(IEntityType);
        }

        /// <summary>
        /// Determines if the given type implements ICollectionAdapter
        /// </summary>
        /// <param name="type"></param>
        
        /// <remarks>The value is determined if the Object implements the IListAdapterType interface</remarks>
        public static bool IsListAdapter(this Type type)
        {
            return type.IsDerivedFrom(IListAdapterType);
        }

        /// <summary>
        /// Determines if the given type implements IValueObject. It is NOTHING to do with IsValueType()!
        /// </summary>
        /// <param name="type"></param>
        
        /// <remarks>The value is determined if the Object implements the IValueObject interface</remarks>
        public static bool IsValueObject(this Type type)
        {
            return type.IsDerivedFrom(IValueObjectType);
        }

        /// <summary>
        /// Determines if the given type implements IAggregateRoot
        /// </summary>
        /// <param name="type"></param>
        
        /// <remarks>The value is determined if the Object implements the IAggregateRoot interface</remarks>
        public static bool IsAggregateRoot(this Type type)
        {
            return type.IsDerivedFrom(IAggregateRootType);
        }

        /// <summary>
        /// Determines if the given type implements IFactory
        /// </summary>
        /// <param name="type"></param>
        
        public static bool IsFactory(this Type type)
        {
            return type.IsDerivedFrom(IFactoryType);
        }

        /// <summary>
        /// Determines if the given type implements IFactory
        /// </summary>
        /// <param name="type"></param>
        /// <param name="realObjectType">The type of Object created by the factory</param>
        
        public static bool IsFactory(this Type type, out Type objectType)
        {
            objectType = null;

            if (type.IsFactory())
            {
                objectType = Fresnel.Utils.TypeExtensions.GetInterfaceGenericType(type, IFactoryType);
            }

            return (objectType != null);
        }

        /// <summary>
        /// Determines if the given type implements IRepository
        /// </summary>
        /// <param name="type"></param>
        
        public static bool IsRepository(this Type type)
        {
            return type.IsDerivedFrom(IRepositoryType);
        }

        /// <summary>
        /// Determines if the given type implements IRepository
        /// </summary>
        /// <param name="type">The type of Object created by the Repository</param>
        
        public static bool IsRepository(this Type type, out Type objectType)
        {
            objectType = null;

            if (type.IsRepository())
            {
                objectType = Fresnel.Utils.TypeExtensions.GetInterfaceGenericType(type, IRepositoryType);
            }

            return (objectType != null);
        }

        /// <summary>
        /// Determines if the given type implements IDomainService
        /// </summary>
        /// <param name="type"></param>
        
        public static bool IsDomainService(this Type type)
        {
            return type.IsDerivedFrom(IDomainServiceType);
        }


        ///// <summary>
        ///// Determines if the given type implements IPresenter
        ///// </summary>
        ///// <param name="type"></param>
        //
        //public static bool IsPresenter(this Type type)
        //{
        //    return type.IsDerivedFrom(IPresenterType);
        //}

        ///// <summary>
        ///// Determines if the given type implements IPresenter
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="realObjectType">The type of Object the presenter handles</param>
        //
        //public static bool IsPresenter(this Type type, out Type realObjectType)
        //{
        //    realObjectType = null;
        //    if (type.IsPresenter())
        //    {
        //        realObjectType = GetInterfaceGenericTypeFrom(type, IPresenterType);
        //    }

        //    return (realObjectType != null);
        //}

        /// <summary>
        /// Returns TRUE if the given type is a candidate for TrueView
        /// </summary>
        /// <param name="realObjectType"></param>
        
        public static bool IsTrackable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                                   s_IsObjectTrackableSpecification.IsSatisfiedBy(objectType).Passed;
        
            return isValid;
        }

        /// <summary>
        /// Returns TRUE if the given type has an IAudit property
        /// </summary>
        /// <param name="realObjectType"></param>
        
        public static bool IsAuditable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                           (objectType.GetConstructor(Type.EmptyTypes) != null) &&
                           s_IsObjectAuditableSpecification.IsSatisfiedBy(objectType).Passed;

            return isValid;
        }

        /// <summary>
        /// Returns TRUE if the given type has an IsValid():bool method
        /// </summary>
        /// <param name="realObjectType"></param>
        
        public static bool IsValidatable(this Type objectType)
        {
            bool isValid = (objectType.DeclaringType == null) &&
                           s_IsObjectValidatableSpecification.IsSatisfiedBy(objectType).Passed;

            return isValid;
        }
        
        
        /// Determines if the given type implements IDataErrorInfo
        /// </summary>
        /// <param name="type"></param>
        
        public static bool IsDataErrorInfo(this Type type)
        {
            return type.IsDerivedFrom(IDataErrorInfoType);
        }
    
    }
}
