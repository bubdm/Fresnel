using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Used to build an Attribute at run-time, usually using values from other attributes
    /// </summary>
    public interface IMissingAttributeBuilder
    {
        bool CanHandle(Type attributeType);

        Attribute BuildFrom(Type parentClass, Type templateType, IEnumerable<Attribute> templateAttributes);
    }

}