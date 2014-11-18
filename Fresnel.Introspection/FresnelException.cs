//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using System;
using System.Runtime.Serialization;

namespace Envivo.Fresnel.Introspection
{
    public class FresnelException : ApplicationException
    {
        public FresnelException()
            : base()
        {
        }

        public  FresnelException(string message)
            : base(message)
        {
        }

        public  FresnelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public  FresnelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

