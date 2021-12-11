namespace BootstrapServer.Models
{
    public class BootstrapOptions
    {
        public List<string> Properties { get; set; }
        public List<HostOption> HostOptions { get; set; }
    }

    public static class BootstrapOptionsExtensions
    {
        public static List<HostOption> Order(this List<HostOption> hosts, Dictionary<string, string> predicates)
        {
            var result = new List<(int, HostOption)>();
            for (var i = 0; i < hosts.Count; i++)
            {
                var hostOption = hosts[i];
                var count = predicates.Count(entry => hostOption.Predicates.ContainsKey(entry.Key) && hostOption.Predicates[entry.Key] == entry.Value);
                result.Add((count, hostOption));
            }

            result = result.OrderByDescending(a => a.Item1).ToList();
            return result.Select(a => a.Item2).ToList();
        }
    }
}
