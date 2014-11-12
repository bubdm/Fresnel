
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{

    public interface IProgressArgs
    {
      
        /// <summary>
        /// Determines if the operation can be cancelled. This is typically set by the consumer, and used within the operation.
        /// </summary>
        bool IsCancellationAllowed { get; set; }

        /// <summary>
        /// Determines if the operation needs to be cancelled. This is typically set by the consumer, and used within the operation.
        /// </summary>
        bool IsCancellationPending { get; set; }

        /// <summary>
        /// How far the operation is from completion
        /// </summary>
        int? PercentComplete { get; set; }

        /// <summary>
        /// The message to report to the consumer
        /// </summary>
        string Message { get; }

    }

}
