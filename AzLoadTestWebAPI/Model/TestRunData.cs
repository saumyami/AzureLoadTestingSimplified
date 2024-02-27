using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunData
    {
        [JsonPropertyName("displayName")]
        public string? display  { get; set; } = null;

        [JsonPropertyName("description")]
        public string? description { get; set; } = null;

        [JsonPropertyName("engineInstances")]
        public int engineInstances { get; set; } = 1;

        [JsonPropertyName("environmentVariables")]
        public LoadTestEnvironmentVariables? loadTestEnvironmentVariables { get; set; } = null;

    }
}
