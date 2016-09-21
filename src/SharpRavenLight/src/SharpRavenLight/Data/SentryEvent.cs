using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.Data
{
    public class SentryEvent
    {
        private Exception exception;
        private SentryMessage sentryMessage;

        public SentryEvent(Exception exception)
        {
            this.exception = exception;
        }

        public SentryEvent(SentryMessage sentryMessage)
        {
            this.sentryMessage = sentryMessage;
        }
    }
}
