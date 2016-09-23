using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRavenLight.Util
{
    public static class UtilExtensions
    {
        public static T JsonStringToObject<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string ObjectToJsonString(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static byte[] Gzip(this string txt)
        {
            var data = Encoding.UTF8.GetBytes(txt);

            using (var stream = new MemoryStream())
            using (var gzip = new GZipStream(stream, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);

                return stream.ToArray();
            }
        }
    }
}
