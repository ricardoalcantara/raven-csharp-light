using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpRavenLight.Util;
using SharpRavenLight.Rest.DTO;

namespace SharpRavenLight
{
    public class RavenClientLight
    {
        private Dsn dsn;

        public RavenClientLight(string dsn)
        {
            this.dsn = new Dsn(dsn);
        }

        public RavenClientLight(Dsn dsn)
        {
            this.dsn = dsn;
        }

        public async Task<string> CaptureAsync(SentryEvent sentryEvent)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId, sentryEvent);

            var sentryApi = new Rest.SentryApi(dsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string Capture(SentryEvent sentryEvent)
        {
            return CaptureAsync(sentryEvent).Result;
        }
    }
}
