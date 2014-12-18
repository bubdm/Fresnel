
using System;
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.Introspection.IoC
{
    /// <summary>
    /// Used to register Domain dependency types (IFactory, IRepository, etc) with an IoC container
    /// </summary>
    public interface IDomainDependencyRegistrar
    {

        /// <summary>
        /// Registers the given Types with an IoC container
        /// </summary>
        /// <param name="types"></param>
        void RegisterTypes(Type[] types);

    }

}
