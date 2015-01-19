using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.Core
{
    public static class IEntityExtensions
    {
        public static bool IsEqualTo(this IEntity a, IEntity b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if (a == null && b == null)
                return false;

            if (a == null && b != null)
                return false;

            if (a != null && b == null)
                return false;

            if (a.GetType() == b.GetType())
                return true;

            if (a.ID == b.ID)
                return true;

            return false;
        }
    }
}