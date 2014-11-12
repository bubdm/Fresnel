using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;
using Envivo.Sample.Model.Objects;

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
