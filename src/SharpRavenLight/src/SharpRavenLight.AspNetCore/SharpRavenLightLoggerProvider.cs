using Microsoft.Extensions.Logging;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpRavenLight.AspNetCore
{
    public class SharpRavenLightLoggerProvider : ILoggerProvider
    {

        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IRavenClient _ravenClient;

        public SharpRavenLightLoggerProvider(Func<string, LogLevel, bool> filter, IRavenClient ravenClient)
        {
            _ravenClient = ravenClient;
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SharpRavenLightLogger(categoryName, _filter, _ravenClient);
        }

        public void Dispose()
        {
            // TODO: dispose managed state (managed objects).
        }
    }
}
