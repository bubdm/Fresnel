using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IConfigurationBuilder
    {
        Attribute BuildFrom(IEnumerable<Attribute> templateAttributes, Type parentClass);
    }

}