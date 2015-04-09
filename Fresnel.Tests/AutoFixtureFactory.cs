using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fresnel.Tests
{
    public class AutoFixtureFactory
    {
        private Random _rnd = new Random((int)DateTime.Now.Ticks);

        public Fixture Create()
        {
            var fixture = new Fixture();

            // Prevent recursion errors:
            var throwingRecursionBehavior = fixture.Behaviors.Single(b => b is ThrowingRecursionBehavior);
            fixture.Behaviors.Remove(throwingRecursionBehavior);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(10));

            fixture.Customizations.Add(new MinMaxConstraintBuilder());

            fixture.Customize<Category>(c => c
                .Without(x => x.Products)
            );

            fixture.Customize<Product>(c => c
                .Without(x => x.Categories)
                .Without(x => x.Stock)
            );

            fixture.Customize<Person>(c => c
                // "Role" is abstract, therefore cannot be created:
                .Without(x => x.Roles)
            );

            fixture.Customize<Employee>(c => c
                .Without(x => x.Territories)
                .Without(x => x.Notes)
            );

            fixture.Customize<Customer>(c => c
                .Without(x => x.Party)
            );

            fixture.Customize<Supplier>(c => c
                .Without(x => x.Party)
            );

            fixture.Customize<Order>(c => c
                .Without(x => x.OrderItems)
            );

            return fixture;
        }

    }

    public class MinMaxConstraintBuilder : ISpecimenBuilder
    {
        private readonly Random _Rnd = new Random((int)DateTime.Now.Ticks);

        public object Create(object request, ISpecimenContext context)
        {
            var result = new NoSpecimen(request);

            var pi = request as PropertyInfo;
            if (pi != null &&
                pi.PropertyType == typeof(string))
            {
                MinLengthAttribute minLengthAttr = null;
                MaxLengthAttribute maxLengthAttr = null;

                if (Attribute.IsDefined(pi, typeof(MinLengthAttribute)))
                {
                    minLengthAttr = (MinLengthAttribute)Attribute.GetCustomAttribute(pi, typeof(MinLengthAttribute));
                }
                if (Attribute.IsDefined(pi, typeof(MaxLengthAttribute)))
                {
                    maxLengthAttr = (MaxLengthAttribute)Attribute.GetCustomAttribute(pi, typeof(MaxLengthAttribute));
                }

                var minLength = minLengthAttr == null ? 0 : minLengthAttr.Length;
                var maxLength = maxLengthAttr == null ? 50 : maxLengthAttr.Length;
                var isRequired = Attribute.IsDefined(pi, typeof(RequiredAttribute));

                return this.GenerateString(minLength, maxLength, isRequired);
            }
            
            return result;
        }

        private string GenerateString(int minLength, int maxLength, bool isRequired)
        {
            const string _Seed = "acbdefghijklymnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789-()Â£$";

            if (!isRequired && _Rnd.Next(100) < 33)
                return null;

            var result = new StringBuilder();

            var length = _Rnd.Next(minLength, maxLength);
            for (var i = 0; i < length; i++)
            {
                var character = _Seed[_Rnd.Next(0, _Seed.Length - 1)];
                result.Append(character);
            }

            return result.ToString();
        }
    }

}
