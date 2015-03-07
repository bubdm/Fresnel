using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IConfigurationBuilder<T>
        where T: BaseConfiguration
    {
        T BuildFrom(Attribute[] attributes);
    }
}