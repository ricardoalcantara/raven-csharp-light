using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.Data
{
    public class SentryEvent
    {
        public Exception Exception { get; set; }
        public SentryMessage SentryMessage { get; set; }

        public SentryEvent(Exception exception)
        {
            this.Exception = exception;
        }

        public SentryEvent(SentryMessage sentryMessage)
        {
            this.SentryMessage = sentryMessage;
        }
    }
}
