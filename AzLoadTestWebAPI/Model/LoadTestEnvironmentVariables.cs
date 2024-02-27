using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class LoadTestEnvironmentVariables
    {
        [JsonPropertyName("bearerToken")]
        public string? bearerToken { get; set; } = null;

        [JsonPropertyName("serverName")]
        public string? serverName { get; set; } = null;

        [JsonPropertyName("queryToTestData")]
        public string? queryToTestData { get; set; } = null;

        [JsonPropertyName("databaseName")]
        public string? databaseName { get; set; } = null;

        [JsonPropertyName("loopCount")]
        public string? loopCount { get; set; } = null;

        [JsonPropertyName("numberOfThreads")]
        public string? numberOfThreads { get; set; } = null;
    }
}
