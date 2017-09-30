using Microsoft.Extensions.Logging;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpRavenLight.AspNetCore
{
    public static class SharpRavenLightLoggerExtensions
    {
        public static ILoggerFactory AddRaven(this ILoggerFactory factory, IRavenClient ravenClient, Func<string, LogLevel, bool> filter = null)
        {
            factory.AddProvider(new SharpRavenLightLoggerProvider(filter, ravenClient));
            return factory;
        }

        public static ILoggerFactory AddRaven(this ILoggerFactory factory, IRavenClient ravenClient, LogLevel minLevel)
        {
            return AddRaven(
                factory,
                ravenClient,
                (_, logLevel) => logLevel >= minLevel);
        }
    }
}
