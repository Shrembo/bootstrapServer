using System.Text.Json.Serialization;

namespace BootstrapServer.Models
{
    public class HostOption
    {
        public List<string> Host { get; set; }
        public int Ttl { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Predicates { get; set; }

        [JsonIgnore]
        public DateTime CacheExpired { get; set; }
    }
}
