using Newtonsoft.Json;

namespace softrobotics.gateway
{
    [JsonObject("CacheOptions")]
    public class CacheOption
    {
        public string Type { get; set; }
        public int ExpirySeconds { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int DatabaseIndex { get; set; }
    }
}