using Envivo.Fresnel.DomainTypes.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    ///
    /// </summary>
    public class EnumValuesFilterSpecification : IQuerySpecification<EnumValues.IndividualOptions>
    {
        private EnumValues.IndividualOptions[] _FilterItems = new EnumValues.IndividualOptions[] { EnumValues.IndividualOptions.Red, EnumValues.IndividualOptions.Blue };

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IQueryable<EnumValues.IndividualOptions> GetResults()
        {
            // The requesting object may be used to determine which results to return
            return _FilterItems.AsQueryable<EnumValues.IndividualOptions>();
        }
    }
}