using System.Text.Json.Serialization;

namespace AzLoadTestWebAPI.Model
{
    public class LoadTestConfiguration
    {
        [JsonPropertyName("engineInstances")]
        public int? engineInstances { get; set; } = 1;

        [JsonPropertyName("splitAllCSVs")]
        public bool? splitAllCSVs { get; set; } = true;
    }
}
