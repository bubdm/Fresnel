using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

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
