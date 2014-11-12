using System.Collections.Generic;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    public class EnumValuesFilterSpecification : IQuerySpecification<EnumValues.IndividualOptions>
    {

        public IEnumerable<EnumValues.IndividualOptions> GetResults()
        {
            // The requesting object may be used to determine which results to return

            return new List<EnumValues.IndividualOptions>() { EnumValues.IndividualOptions.Red, EnumValues.IndividualOptions.Blue };
        }

    }
}
