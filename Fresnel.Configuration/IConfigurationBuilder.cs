using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IConfigurationBuilder
    {
        BaseConfiguration BuildFrom(IEnumerable<Attribute> attributes);
    }

}