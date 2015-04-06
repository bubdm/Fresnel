using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Applies checks against a Domain object, to ensure it is fit for persisting
    /// </summary>
    public interface IConsistencyCheck<T>
    {

        AggregateException Check(T obj);

    }
}