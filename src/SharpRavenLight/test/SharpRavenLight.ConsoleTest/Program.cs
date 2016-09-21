using SharpRavenLight.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.ConsoleTest
{
    public class Program
    {
        private const string dsnUrl = "https://7d6466e66155431495bdb4036ba9a04b:4c1cfeab7ebd4c1cb9e18008173a3630@app.getsentry.com/3739";

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
