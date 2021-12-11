using BootstrapServer.Models;
using Microsoft.Extensions.Options;

namespace BootstrapServer
{
    public class BootstrapRoutingService
    {
        private readonly ILogger<BootstrapRoutingService> logger;
        private readonly BootstrapOptions options;

        public BootstrapRoutingService(ILoggerFactory logger, IOptions<BootstrapOptions> options)
        {
            this.logger = logger.CreateLogger<BootstrapRoutingService>();
            this.options = options.Value;
        }


        public HostOption Process(IQueryCollection queryParams)
        {
            var predicates = ProcessParameters(queryParams);
            var key = CacheService.GenerateCacheKey(predicates);

            if (!CacheService.TryGet(key, out var value))
            {
                var matchesCount = int.MinValue;
                for (int i = 0; i < options.HostOptions.Count; i++)
                {
                    var hostOption = options.HostOptions[i];
                    var count = predicates.Count(entry => hostOption.Predicates.ContainsKey(entry.Key) && hostOption.Predicates[entry.Key] == entry.Value);
                    if (count > matchesCount)
                    {
                        matchesCount = count;
                        value = hostOption;
                    }
                }
                CacheService.Add(key, value);
            }
            return CacheService.Filter(value);
        }


        private Dictionary<string, string> ProcessParameters(IQueryCollection queryParams)
        {
            var predicates = new Dictionary<string, string>();
            foreach (var property in options.Properties)
            {
                if (queryParams.Keys.Contains(property))
                {
                    predicates.Add(property, queryParams[property]);
                    logger.LogDebug($"Request contains property: {property}, Value: {queryParams[property]}");
                }
                else
                {
                    predicates.Add(property, "*");
                    logger.LogDebug($"Request does not contains property: {property}, default value: *");
                }
            }
            return predicates;
        }
    }
}
