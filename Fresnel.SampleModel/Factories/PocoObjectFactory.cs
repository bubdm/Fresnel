using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;

namespace Envivo.Sample.Model.Factories
{
    public class PocoObjectFactory : IFactory<PocoObject>
    {
        public PocoObject Create()
        {
            return new PocoObject();
        }

        public PocoObject Create(PocoObject parent)
        {
            var result = new PocoObject();

            result.NormalText = "Owned by " + parent.ID.ToString();

            return result;
        }
    }
}