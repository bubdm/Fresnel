using System;
using System.Collections.Generic;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Core
{
    public static class SequentialIdGenerator
    {
        private static long _ID = long.MinValue;

        public static long Next
        {
            get { return ++_ID; }
        }

    }
}
