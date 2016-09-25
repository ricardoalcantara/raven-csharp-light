using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRavenLight.Util
{
    internal static class UtilExtensions
    {
        internal static T JsonStringToObject<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        internal static string ObjectToJsonString(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        internal static byte[] Gzip(this string txt)
        {
            byte[] data = Encoding.UTF8.GetBytes(txt);

            using (MemoryStream stream = new MemoryStream())
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);

                return stream.ToArray();
            }
        }
    }
}
