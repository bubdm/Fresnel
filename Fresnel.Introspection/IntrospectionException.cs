//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using System;
using System.Runtime.Serialization;

namespace Envivo.Fresnel.Introspection
{
    public class IntrospectionException : ApplicationException
    {
        public IntrospectionException()
            : base()
        {
        }

        public IntrospectionException(string message)
            : base(message)
        {
        }

        public IntrospectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public IntrospectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

