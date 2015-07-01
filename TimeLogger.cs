using System;
using System.Diagnostics;
using log4net;

namespace Helpers
{
    /// <summary>
    /// Logs operations that are taking longer than 500 miliseconds:
    /// 
    /// Example:
    /* 
       using (new TimeLogger(Log, message))  {
                Thread.Sleep(1000);
            }
     */
    /// </summary>
    public class TimeLogger : IDisposable
    {
        private readonly DateTime _start = DateTime.Now;
        private readonly ILog _log;
        private string _message;

        public TimeLogger(ILog log, string message = "")
        {
            _log = log;
            _message = message;
        }

        public void WriteMethodEntry()
        {
            var stackTrace = new StackTrace();
            string methodName = stackTrace.GetFrame(2).GetMethod().Name;

            _log.Debug(String.Format("Method {0} Entered", methodName));
        }

        public void WriteMethodExit()
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(2).GetMethod().Name;

            var durationString = "ERROR";

            var duration = DateTime.Now - _start;
            durationString = Convert.ToString(duration);
            if (duration > TimeSpan.FromMilliseconds(500))
            {
                _log.WarnFormat("Method {0} Duration: {1} [{2}]", methodName, duration, _message);
            }

            _log.Debug(String.Format("Method {0} Exited - Duration: {1} ({2})", methodName, durationString, _message));
        }

        public void Dispose()
        {
            WriteMethodExit();
        }
    }
}
