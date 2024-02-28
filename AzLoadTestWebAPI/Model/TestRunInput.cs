using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunInput
    {
        [JsonPropertyName("subscriptionId")]
        public string? subscriptionId { get; set; } = null;

        [JsonPropertyName("resourceGroup")]
        public string? resourceGroup { get; set; } = null;

        [JsonPropertyName("azureLoadTestingResourceName")]
        public string? azureLoadTestingResourceName { get; set; } = null;

        [JsonPropertyName("loadTestRuns")]
        public IEnumerable<TestRunData>? loadTestRuns { get; set; } = null;
    }

}
