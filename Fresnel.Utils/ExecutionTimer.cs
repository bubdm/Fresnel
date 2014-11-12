using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Utils
{
    public class ExecutionTimer : IDisposable
    {
        #if DEBUG
        private string _Title = "Execution";
        private Stopwatch _sw = new Stopwatch();

        public ExecutionTimer()
        {
            _sw.Start();
        }

        public ExecutionTimer(string title)
        {
            _Title = title;
            _sw.Start();
        }

        public TimeSpan GetElapsedTime()
        {
            return _sw.Elapsed;
        }

        public void Dispose()
        {
            _sw.Stop();
            Trace.TraceInformation(string.Concat("EXECUTION TIMER: ", _Title, " took ", _sw.ElapsedMilliseconds, "ms"));
        }
        #else
        public ExecutionTimer()
        {
        }

        public ExecutionTimer(string title)
        {
        }

        public void Dispose()
        {
        }
#endif
    }
}
