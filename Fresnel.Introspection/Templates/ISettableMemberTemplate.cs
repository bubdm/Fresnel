using System;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.Introspection.Templates
{
    public interface ISettableMemberTemplate
    {
        AttributesMap Attributes { get; }

        bool IsDomainObject { get; }

        bool IsCollection { get; }

        bool IsNonReference { get; }

        Type ValueType { get; }

        IClassTemplate InnerClass { get; }
    }
}