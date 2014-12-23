

using System;
namespace Envivo.Fresnel.Utils
{
    public class SettableClock : IClock
    {
        private TimeSpan _OffsetFromNow;

        public DateTime Now
        {
            get { return DateTime.Now.Add(_OffsetFromNow); }
            set { _OffsetFromNow = DateTime.Now.Subtract(value); }
        }
    }
}
