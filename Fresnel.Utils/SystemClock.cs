

using System;
namespace Envivo.Fresnel.Utils
{
    public class SystemClock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
            set { }
        }
    }
}
