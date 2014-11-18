using System;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents a Non-Reference Type (e.g. primitives, structs, and non-reference types)
    /// </summary>
    
    public class NonReferenceTemplate : BaseClassTemplate
    {
        public NonReferenceTemplate()
        {
            this.KindOf = TypeKind.Unidentified;
        }

        public TypeKind KindOf { get; internal set; }

    }
}
