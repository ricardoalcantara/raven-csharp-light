using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpRavenLight.Configuration;
using SharpRavenLight.Data;

namespace SharpRavenLight
{
    public partial class RavenClientLight : IRavenClientLight
    {
        public bool Compression { get; set; } = false;
        public string Environment { get; set; }
        public string Logger { get; set; } = "root";
        public string Release { get; set; }
        public IDictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
        public Dsn CurrentDsn { get; }

        public RavenClientLight(string dsn)
            : this(new Dsn(dsn))
        {
        }

        public RavenClientLight(Dsn dsn)
        {
            this.CurrentDsn = dsn;
        }

        public RavenClientLight(ConfigurationOptions options)
            : this(options.dsn)
        {
        }

        public string Capture(SentryEvent sentryEvent)
        {
            return CaptureAsync(sentryEvent).Result;
        }

        public async Task<string> CaptureAsync(SentryEvent sentryEvent)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, sentryEvent);

            var sentryApi = new Rest.SentryApi(CurrentDsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string CaptureEvent(Exception e)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, e);

            var sentryApi = new Rest.SentryApi(CurrentDsn);
            var result = sentryApi.StoreAsync(jsonPacket).Result;

            return result.id;
        }

        public string CaptureEvent(Exception e, Dictionary<string, string> tags)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, e);
            jsonPacket.Tags = tags;

            var sentryApi = new Rest.SentryApi(CurrentDsn);
            var result = sentryApi.StoreAsync(jsonPacket).Result;

            return result.id;
        }

        public string CaptureException(Exception exception, SentryMessage message = null, ErrorLevel level = ErrorLevel.Error, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            return CaptureExceptionAsync(exception, message, level, tags, fingerprint, extra).Result;
        }

        public async Task<string> CaptureExceptionAsync(Exception exception, SentryMessage message = null, ErrorLevel level = ErrorLevel.Error, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, exception)
            {
                MessageObject = message,
                Level = level,
                Tags = tags,
                Fingerprint = fingerprint,
                Extra = extra
            };

            var sentryApi = new Rest.SentryApi(CurrentDsn);
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string CaptureMessage(SentryMessage message, ErrorLevel level = ErrorLevel.Info, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            return CaptureMessageAsync(message, level, tags, fingerprint, extra).Result;
        }

        public async Task<string> CaptureMessageAsync(SentryMessage message, ErrorLevel level = ErrorLevel.Info, IDictionary<string, string> tags = null, string[] fingerprint = null, object extra = null)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId)
            {
                MessageObject = message,
                Level = level,
                Tags = tags,
                Fingerprint = fingerprint,
                Extra = extra
            };

            var sentryApi = new Rest.SentryApi(CurrentDsn);
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
