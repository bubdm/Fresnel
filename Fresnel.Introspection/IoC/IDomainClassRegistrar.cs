
using System;
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.Introspection.IoC
{
    /// <summary>
    /// Used to register Domain Class types with an IoC container
    /// </summary>
    public interface IDomainClassRegistrar
    {

        /// <summary>
        /// Registers the given Types so they can be created by an IoC container
        /// </summary>
        /// <param name="domainClassTypes"></param>
        void RegisterTypes(Type[] domainClassTypes);

    }

}
