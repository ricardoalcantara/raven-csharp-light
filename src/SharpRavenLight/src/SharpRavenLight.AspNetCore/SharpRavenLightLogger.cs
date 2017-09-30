using Microsoft.Extensions.Logging;
using SharpRaven;
using System;

namespace SharpRavenLight.AspNetCore
{
    public class SharpRavenLightLogger : ILogger
    {

        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;
        private IRavenClient _ravenClient;

        public SharpRavenLightLogger(string categoryName, Func<string, LogLevel, bool> filter, IRavenClient ravenClient)
        {
            _categoryName = categoryName;
            _filter = filter;
            _ravenClient = ravenClient;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Data.ErrorLevel errorLevel = Data.ErrorLevel.Debug;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Critical:
                    errorLevel = Data.ErrorLevel.Fatal;
                    break;
                case LogLevel.Error:
                    errorLevel = Data.ErrorLevel.Error;
                    break;
                case LogLevel.Warning:
                    errorLevel = Data.ErrorLevel.Warning;
                    break;
                case LogLevel.Information:
                    errorLevel = Data.ErrorLevel.Info;
                    break;
                case LogLevel.Debug:
                    errorLevel = Data.ErrorLevel.Debug;
                    break;
            }

            _ravenClient.CaptureAsync(new Data.SentryEvent(exception)
            {
                Message = message,
                Level = errorLevel,
                Extra = new
                {
                    EventId = eventId
                }
            });
        }
    }
}
