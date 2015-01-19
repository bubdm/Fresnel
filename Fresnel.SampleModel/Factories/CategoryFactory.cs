using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;

namespace Envivo.Sample.Model.Factories
{
    public class CategoryFactory : IFactory<Category>
    {
        public Category Create()
        {
            return new Category();
        }
    }
}