using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpRavenLight.Configuration;
using SharpRavenLight.Data;
using SharpRavenLight.Util;
using SharpRaven;

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

        protected Lazy<Rest.SentryApi> sentryApiLazy;

        protected Rest.SentryApi sentryApi => sentryApiLazy.Value;

        public RavenClientLight(string dsn, string environment = "")
            : this(new Dsn(dsn), environment)
        {
        }

        public RavenClientLight(Dsn dsn, string environment)
        {

            this.CurrentDsn = dsn;
            #if NET_CORE
                this.Environment = SystemUtil.IsNullOrWhiteSpace(environment)
                    ? System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    : environment;
            #else
                this.Environment = environment;
            #endif

            this.sentryApiLazy = new Lazy<Rest.SentryApi>(() => {
                return new Rest.SentryApi(this.CurrentDsn);
            });
        }

        public RavenClientLight(ConfigurationOptions options)
            : this(options.dsn, options.environment)
        {
        }

        public string Capture(SentryEvent sentryEvent)
        {
            return CaptureAsync(sentryEvent).Result;
        }

        public async Task<string> CaptureAsync(SentryEvent sentryEvent)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, sentryEvent);
            
            var result = await sentryApi.StoreAsync(jsonPacket);

            return result.id;
        }

        public string CaptureEvent(Exception e)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, e);
            jsonPacket.Environment = Environment;

            
            var result = sentryApi.StoreAsync(jsonPacket).Result;

            return result.id;
        }

        public string CaptureEvent(Exception e, Dictionary<string, string> tags)
        {
            var jsonPacket = new JsonPacket(CurrentDsn.ProjectId, e);
            jsonPacket.Tags = tags;
            jsonPacket.Environment = Environment;

           
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
                Extra = extra,
                Environment = this.Environment
            };

           
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
                Extra = extra,
                Environment = this.Environment
            };

            
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
