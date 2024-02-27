using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunData
    {
        [JsonPropertyName("displayName")]
        public string? display  { get; set; } = null;

        [JsonPropertyName("description")]
        public string? description { get; set; } = null;

        [JsonPropertyName("testId")]
        public string? testId { get; set; } = null;

        [JsonPropertyName("loadTestConfiguration")]
        public LoadTestConfiguration? loadTestConfiguration { get; set; } = null;

        [JsonPropertyName("environmentVariables")]
        public Dictionary<string,string>? loadTestEnvironmentVariables { get; set; } = null;

    }
}
