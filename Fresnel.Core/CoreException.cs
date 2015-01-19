using System;
using System.Runtime.Serialization;

namespace Envivo.Fresnel.Core
{
    public class CoreException : ApplicationException
    {
        public CoreException()
            : base()
        {
        }

        public CoreException(string message)
            : base(message)
        {
        }

        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}