using SharpRavenLight.Rest.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.ConsoleTest
{
    public class Program
    {
        private const string dsnUrl = "https://23b1369357f54bbf8fc9a44dc042f1bb:a53a456bc7dd494a9f2c120eb1557c3a@sentry.io/100676";

        static void Main(string[] args)
        {
            var client = new RavenClientLight(dsnUrl);
            client.Capture(new SentryEvent(new SentryMessage("Hello world")));

            try
            {
                DivideByZero();
            }
            catch (Exception exception)
            {
                client.Capture(new SentryEvent(exception));
            }
        }

        private static void DivideByZero(int stackFrames = 10)
        {
            if (stackFrames == 0)
            {
                var a = 0;
                var b = 1 / a;
            }
            else
                DivideByZero(--stackFrames);
        }
    }
}
