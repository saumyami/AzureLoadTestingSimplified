using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunDataOutput
    {
        public TestRunDataOutput(TestRunData testRunData)
        {
            display = testRunData.display;
            description = testRunData.description;
            testId = testRunData.testId;
            loadTestConfiguration = testRunData.loadTestConfiguration;
            loadTestEnvironmentVariables = testRunData.loadTestEnvironmentVariables;
        }
        [JsonPropertyName("displayName")]
        public string? display { get; set; } = null;

        [JsonPropertyName("description")]
        public string? description { get; set; } = null;

        [JsonPropertyName("testId")]
        public string? testId { get; set; } = null;

        [JsonPropertyName("results")]
        public Dictionary<string, double> result { get; set; } = new Dictionary<string, double>();

        [JsonPropertyName("loadTestConfiguration")]
        public LoadTestConfiguration? loadTestConfiguration { get; set; } = null;

        [JsonPropertyName("environmentVariables")]
        public Dictionary<string, string>? loadTestEnvironmentVariables { get; set; } = null;
    }
}
