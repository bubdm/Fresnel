using System.Collections.Generic;
using Envivo.DomainTypes;

namespace Envivo.Sample.Model.BasicTypes
{
    public class EnumValuesFilterSpecification : IQuerySpecification<EnumValues.IndividualOptions>
    {

        public IEnumerable<EnumValues.IndividualOptions> GetResults(object requester)
        {
            // The requesting object may be used to determine which results to return

            return new List<EnumValues.IndividualOptions>() { EnumValues.IndividualOptions.Red, EnumValues.IndividualOptions.Blue};
        }

    }
}
