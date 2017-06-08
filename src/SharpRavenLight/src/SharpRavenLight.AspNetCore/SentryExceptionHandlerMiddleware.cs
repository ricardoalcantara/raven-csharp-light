using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SharpRavenLight.AspNetCore
{
    public class SentryExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IRavenClientLight _ravenClient;
        public SentryExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IRavenClientLight ravenClient)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SentryExceptionHandlerMiddleware>();
            _ravenClient = ravenClient;
        }

        public async Task Invoke(HttpContext context)
        {
            await RavenClientLight.CatchAllUnhandledExceptionAsync((ravenClient) =>
            {
                return _next.Invoke(context);
            }, _ravenClient);            
        }
    }
}
