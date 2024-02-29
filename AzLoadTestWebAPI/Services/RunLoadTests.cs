using AzLoadTestWebAPI.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzLoadTestWebAPI.Services
{
    public class RunLoadTests
    {
        HttpClient _httpClient;
        string _loadTestAccessToken;
        string _mgmtAccessToken;
        string azureLoadTestResouceEndpoint;
        AzureAuthClient _azureAuthClient;
        private readonly IConfiguration _configuration;
        string TestRunId;

        public RunLoadTests(HttpClient httpClient, AzureAuthClient azureAuthClient, IConfiguration configuration) 
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(100000);
            _loadTestAccessToken = "";
            _mgmtAccessToken = "";
            _configuration = configuration;
            _azureAuthClient = azureAuthClient;
            azureLoadTestResouceEndpoint = "986cc001-5bc8-4f4d-80cc-27bf3fa34763.eastus.cnt-prod.loadtesting.azure.com";
            TestRunId = "";
        }
        public async Task CreateAuthClient()
        {
            (_loadTestAccessToken, _mgmtAccessToken) = await _azureAuthClient.GetAccessToken();
        }

        private async Task<string> GetAzureLoadTestingDataPlaneEndpoint(TestRunInput testRunInput)
        {
            string apiUrl = $"https://management.azure.com/subscriptions/{testRunInput.subscriptionId}/resourceGroups/{testRunInput.resourceGroup}/providers/Microsoft.LoadTestService/loadTests/{testRunInput.azureLoadTestingResourceName}?api-version=2022-12-01";
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mgmtAccessToken);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var resContent = JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync())!;
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(resContent["properties"]))!["dataPlaneURI"];
        }
        public async Task<List<TestRunDataOutput>> CreateSequential(TestRunInput testRunInput)
        {
            await CreateAuthClient();
            azureLoadTestResouceEndpoint = await GetAzureLoadTestingDataPlaneEndpoint(testRunInput);
            var testRunDataOutputList = new List<TestRunDataOutput>();
            
                for(int i = 0; i< testRunInput.loadTestRuns?.ToList().Count; i++)
                {
                    await CreateLoadTestRun(testRunInput.loadTestRuns?.ToList()[i]!);
                    bool success = false;
                    double p90 = 0;
                    do
                    {
                        var response = await GetLoadTest();
                        var resObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(response)!;
                        string currStatus = resObj["status"]?.ToString()!;
                        success = currStatus == "DONE" ;
                        if (!success)
                        {
                            await Task.Delay(TimeSpan.FromMinutes(_configuration.GetValue<double>("RetryDelayTime")));
                        }
                    } 
                    while (!success);

                    await Task.Delay(TimeSpan.FromMinutes(_configuration.GetValue<double>("RetryDelayTime")));
                    var res = await GetLoadTest();
                    var resObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(res)!;
                    while (!resObject.ContainsKey("testRunStatistics"))
                    {
                        await Task.Delay(TimeSpan.FromMinutes(_configuration.GetValue<double>("RetryDelayTime")));
                        res = await GetLoadTest();
                        resObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(res)!;
                    }
                    var TestStats = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(resObject["testRunStatistics"]))!["Total"];
                    p90 = (double)JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(TestStats))!["pct1ResTime"];

                    var testRunOutputData = new TestRunDataOutput(testRunInput.loadTestRuns?.ToList()[i]!);
                    testRunOutputData.result.Add("P90" , p90);
                    testRunDataOutputList.Add(testRunOutputData);
                }
            return testRunDataOutputList;
        }

        private async Task<string> GetLoadTest()
        {
            string apiUrl = $"https://{azureLoadTestResouceEndpoint}/test-runs/{TestRunId}?api-version=2022-11-01";
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loadTestAccessToken);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string resContent = await response.Content.ReadAsStringAsync();
            return resContent;
        }

        private async Task CreateLoadTestRun(TestRunData data)
        {
            TestRunId = data.testId + Guid.NewGuid();
            string apiUrl = $"https://{azureLoadTestResouceEndpoint}/test-runs/{TestRunId}?api-version=2022-11-01";
            var reqContent = JsonConvert.SerializeObject(data);
            HttpContent httpContent = new StringContent(reqContent, Encoding.UTF8, "application/merge-patch+json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), apiUrl)
            {
                Content = httpContent
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loadTestAccessToken);
            
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string resContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"response: {resContent}");
            }
            else
            {
                Console.WriteLine($"{response.StatusCode}");
            }

        }
    }
}
  