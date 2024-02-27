using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class TestRunInput
    {
        [JsonPropertyName("testData")]
        public IEnumerable<TestRunData>? testData { get; set; } = null;
    }

}
