
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System;
using Fresnel.DomainTypes.Interfaces;

namespace Fresnel.DomainTypes
{
    public class ProgressArgs : EventArgs, IProgressArgs
    {

        /// <summary>
        /// Determines if the operation can be cancelled. This is typically set by the consumer, and used within the operation.
        /// </summary>
        public bool IsCancellationAllowed { get; set; }

        /// <summary>
        /// Determines if the operation needs to be cancelled. This is typically set by the consumer, and used within the operation.
        /// </summary>
        public bool IsCancellationPending { get; set; }

        /// <summary>
        /// How far the operation is from completion
        /// </summary>
        public int? PercentComplete { get; set; }

        /// <summary>
        /// The message to report to the consumer
        /// </summary>
        public string Message { get; set; }

    }
}
