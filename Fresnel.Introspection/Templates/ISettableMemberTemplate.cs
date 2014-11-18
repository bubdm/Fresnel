
namespace Envivo.Fresnel.Introspection.Templates
{

    public interface ISettableMemberTemplate
    {

        bool IsDomainObject { get; }

        bool IsCollection { get; }

        bool IsNonReference { get; }

    }
}
