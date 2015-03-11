using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Used to build an Attribute at run-time, usually using values from other attributes
    /// </summary>
    public interface IMissingAttributeBuilder
    {
        Attribute BuildFrom(IEnumerable<Attribute> templateAttributes, Type parentClass);
    }

}