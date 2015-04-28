using System;
using System.ComponentModel;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConsistencyCheck : IDomainDependency
    { }

    /// <summary>
    /// Applies checks against a Domain object, to ensure it is fit for persisting
    /// </summary>
    public interface IConsistencyCheck<T> : IConsistencyCheck
    {

        AggregateException Check(T obj);

    }
}