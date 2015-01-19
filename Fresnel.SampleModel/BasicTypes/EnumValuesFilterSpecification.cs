using Envivo.Fresnel.DomainTypes.Interfaces;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    ///
    /// </summary>
    public class EnumValuesFilterSpecification : IQuerySpecification<EnumValues.IndividualOptions>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EnumValues.IndividualOptions> GetResults()
        {
            // The requesting object may be used to determine which results to return

            return new List<EnumValues.IndividualOptions>() { EnumValues.IndividualOptions.Red, EnumValues.IndividualOptions.Blue };
        }
    }
}