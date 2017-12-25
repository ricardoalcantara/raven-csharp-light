using SharpRavenLight.Data;
using SharpRavenLight.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharpRavenLight.Rest
{
    public class SentryApi
    {
        private const string USER_AGENT = "SharpRavenLight/1.0.0";
        private const int SENTRY_VERSION = 7;
        private readonly Dsn dsn;
        public bool Compress { get; }

        public TimeSpan Timeout { get; set; }

        protected HttpClient httpClient  = new HttpClient();

        private string SentryAuth
        {
            get
            {
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                return $"Sentry sentry_version={SENTRY_VERSION}, sentry_client={USER_AGENT}, sentry_timestamp={timestamp}, sentry_key={dsn.Key}, sentry_secret={dsn.Secret}";
            }
        }

        public SentryApi(Dsn dsn, bool compress = false)
            : this(dsn, TimeSpan.FromSeconds(15), compress)
        {
        }

        public SentryApi(Dsn dsn, TimeSpan timeout, bool compress = false)
        {
            this.dsn = dsn;
            Timeout = timeout;
            Compress = compress;

            InitHttpClient();
        }

        public void InitHttpClient() {
            httpClient.Timeout = Timeout;
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-Sentry-Auth", SentryAuth);
            httpClient.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);

            if (Compress)
            {
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            }
        }

        public async Task<SentryStoreResponse> StoreAsync(JsonPacket jsonPacket)
        {
            

            var content = Compress
                ? new ByteArrayContent(jsonPacket.ObjectToJsonString().Gzip())
                : new StringContent(jsonPacket.ObjectToJsonString());

            var httpResponse = await httpClient.PostAsync($"https://{dsn.Url}/api/{dsn.ProjectId}/store/", content);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var storeResponse = responseContent.JsonStringToObject<SentryStoreResponse>();
            return storeResponse;
        }
    }
}
