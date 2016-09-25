using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpRavenLight.Util;
using SharpRavenLight.Data;

namespace SharpRavenLight
{
    public class RavenClientLight : IRavenClient
    {
        private Dsn dsn;

        public bool Compression { get; set; } = false;
        public string Environment { get; set; }
        public string Logger { get; set; } = "root";
        public string Release { get; set; }
        public IDictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        public Dsn CurrentDsn
        {
            get
            {
                return dsn;
            }
        }

        public RavenClientLight(string dsn)
        {
            this.dsn = new Dsn(dsn);
        }

        public RavenClientLight(Dsn dsn)
        {
            this.dsn = dsn;
        }

        public string Capture(SentryEvent sentryEvent)
        {
            return CaptureAsync(sentryEvent).Result;
        }

        public async Task<string> CaptureAsync(SentryEvent sentryEvent)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId, sentryEvent);

            var sentryApi = new Rest.SentryApi(dsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string CaptureEvent(Exception e)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId, e);

            var sentryApi = new Rest.SentryApi(dsn);
            var result = sentryApi.StoreAsync(jsonPacket).Result;

            return result.id;
        }

        public string CaptureEvent(Exception e, Dictionary<string, string> tags)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId, e);
            jsonPacket.Tags = tags;

            var sentryApi = new Rest.SentryApi(dsn);
            var result = sentryApi.StoreAsync(jsonPacket).Result;

            return result.id;
        }

        public string CaptureException(Exception exception, SentryMessage message = null, ErrorLevel level = ErrorLevel.Error, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            return CaptureExceptionAsync(exception, message, level, tags, fingerprint, extra).Result;
        }

        public async Task<string> CaptureExceptionAsync(Exception exception, SentryMessage message = null, ErrorLevel level = ErrorLevel.Error, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId, exception)
            {
                MessageObject = message,
                Level = level,
                Tags = tags,
                Fingerprint = fingerprint,
                Extra = extra
            };

            var sentryApi = new Rest.SentryApi(dsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string CaptureMessage(SentryMessage message, ErrorLevel level = ErrorLevel.Info, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            return CaptureMessageAsync(message, level, tags, fingerprint, extra).Result;
        }

        public async Task<string> CaptureMessageAsync(SentryMessage message, ErrorLevel level = ErrorLevel.Info, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            var jsonPacket = new JsonPacket(dsn.ProjectId)
            {
                MessageObject = message,
                Level = level,
                Tags = tags,
                Fingerprint = fingerprint,
                Extra = extra
            };

            var sentryApi = new Rest.SentryApi(dsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public static void CatchAllUnhandledException(Action<IRavenClient> action, IRavenClient ravenClent)
        {
            try
            {
                action(ravenClent);
            }
            catch (Exception ex)
            {
                ravenClent.Capture(new SentryEvent(ex));
                throw;
            }
        }
    }
}
