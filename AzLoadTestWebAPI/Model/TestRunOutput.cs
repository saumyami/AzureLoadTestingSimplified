using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunOutput
    {
        public TestRunOutput(TestRunInput testRunInput)
        {
            subscriptionId = testRunInput.subscriptionId;
            resourceGroup = testRunInput.resourceGroup;
            azureLoadTestingResourceName = testRunInput.azureLoadTestingResourceName;
        }
        [JsonPropertyName("subscriptionId")]
        public string? subscriptionId { get; set; } = null;

        [JsonPropertyName("resourceGroup")]
        public string? resourceGroup { get; set; } = null;

        [JsonPropertyName("azureLoadTestingResourceName")]
        public string? azureLoadTestingResourceName { get; set; } = null;

        [JsonPropertyName("loadTestRuns")]
        public List<TestRunDataOutput> loadTestRuns { get; set; } = new List<TestRunDataOutput>();
    }
}
