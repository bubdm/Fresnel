using System;
using System.Runtime.Serialization;

namespace Envivo.Fresnel.UiCore
{
    public class UiCoreException : ApplicationException
    {
        public UiCoreException()
            : base()
        {
        }

        public UiCoreException(string message)
            : base(message)
        {
        }

        public UiCoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UiCoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

