using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.Data
{
    public class SentryMessage
    {
        private string message;

        public SentryMessage(string message)
        {
            this.message = message;
        }

        public SentryMessage(string format, params object[] args)
        {
            message = string.Format(format, args);
        }
    }
}
