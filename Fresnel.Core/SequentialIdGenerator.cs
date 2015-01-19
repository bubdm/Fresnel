namespace Envivo.Fresnel.Core
{
    public static class SequentialIdGenerator
    {
        private static long _ID = 0;

        public static long Next
        {
            get { return ++_ID; }
        }
    }
}