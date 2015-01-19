using System;
using System.Runtime.Serialization;

namespace Envivo.Fresnel.Configuration
{
    public class ConfigurationException : ApplicationException
    {
        public ConfigurationException()
            : base()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}