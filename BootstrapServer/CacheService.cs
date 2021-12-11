using BootstrapServer.Models;

namespace BootstrapServer
{
    public static class CacheService
    {
        private static Dictionary<string, HostOption> QueriesCache = new Dictionary<string, HostOption>();

        private static List<string> FailedHosts = new List<string>();

        public static void Add(string key, HostOption value)
        {
            value.CacheExpired = DateTime.UtcNow.AddMinutes(60);
            QueriesCache.TryAdd(key, value);
        }
        public static bool TryGet(string key, out HostOption value)
        {
            value = default(HostOption);
            if (QueriesCache.ContainsKey(key))
            {
                value = QueriesCache[key];
                if (DateTime.UtcNow > value.CacheExpired)
                {
                    value = default(HostOption);
                    return false;
                }
                return true;
            }
            return false;
        }

        public static HostOption Filter(HostOption value)
        {
            var result = new HostOption
            {
                Ttl = value.Ttl,
                Host = new List<string>()
            };

            var count = value.Host.Count;
            for (int i = 0; i < count; i++)
            {
                if (!FailedHosts.Contains(value.Host[i]))
                    result.Host.Add(value.Host[i]);
            }
            return result;
        }

        public static List<string> GetHosts()
        {
            return QueriesCache.Values.ToList().SelectMany(v => v.Host).ToList();
        }
        public static string GenerateCacheKey(Dictionary<string, string> predicate)
        {
            var key = string.Empty;
            predicate.Values.ToList().ForEach(v => key = $"{key}{v}");
            return key;
        }

        public static void AddToFailed(string host)
        {
            if (!FailedHosts.Contains(host))
                FailedHosts.Add(host);
        }

        public static void RemoveFromFailed(string host)
        {
            if (FailedHosts.Contains(host))
                FailedHosts.Remove(host);
        }
    }
}
