using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Applies checks against an Entity, to ensure it is fit for persisting
    /// </summary>
    public interface IConsistencyCheck<T>
        where T: IEntity
    {
        
        AggregateException ApplyTo(T obj);

    }
}