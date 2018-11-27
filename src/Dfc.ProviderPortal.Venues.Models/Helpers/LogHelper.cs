
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;


namespace Dfc.ProviderPortal.Venues.Storage
{
    public class LogHelper : ILogger
    {
        //public class MyTraceWriter : TraceWriter {
        //    public MyTraceWriter(TraceLevel traceLevel) { }
        //    public override void Trace(TraceEvent traceEvent) { }
        //}

        private TraceWriter _tw = null;
        private ILogger _log = null;

        public LogHelper(ILogger log) { _log = log; }
        public LogHelper(TraceWriter tw) { _tw = tw; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (_log != null)
                _log.Log(logLevel, eventId, state, exception, formatter);
            else if (_tw != null)
                _tw.Info(formatter.ToString());
        }

        public bool IsEnabled(LogLevel logLevel) { return true; }

        public IDisposable BeginScope<TState>(TState state) { return null; }
    }
}
