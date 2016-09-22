using System;
using System.Text.RegularExpressions;

namespace SharpRavenLight
{
    public class Dsn
    {
        private string dsn;

        public Dsn(string dsn)
        {
            // //([^:]+):([^@]+)@([^/]+)/(\d+)
            var regexFilter = new Regex(@"//([^:]+):([^@]+)@([^/]+)/(\d+)");
            var matchFilter = regexFilter.Match(dsn);

            if (matchFilter.Success && matchFilter.Groups.Count == 5)
            {
                this.dsn = dsn;
                Key = matchFilter.Groups[1].Value;
                Secret = matchFilter.Groups[2].Value;
                Url = matchFilter.Groups[3].Value;
                ProjectId = matchFilter.Groups[4].Value;
            }
            else
                throw new ArgumentException(dsn ?? "null");

        }

        public string Key { get; internal set; }
        public string ProjectId { get; internal set; }
        public string Secret { get; internal set; }
        public string Url { get; private set; }
    }
}