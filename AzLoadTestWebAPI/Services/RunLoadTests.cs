using AzLoadTestWebAPI.Model;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AzLoadTestWebAPI.Services
{
    public class RunLoadTests
    {
        HttpClient _httpClient;
        string azureLoadTestResouceEndpoint;
        AzureAuthClient _azureAuthClient;
        string TestRunId;

        public RunLoadTests(HttpClient httpClient, AzureAuthClient azureAuthClient) 
        {
            _httpClient = httpClient;
            _azureAuthClient = azureAuthClient;
            azureLoadTestResouceEndpoint = "986cc001-5bc8-4f4d-80cc-27bf3fa34763.eastus.cnt-prod.loadtesting.azure.com";
            TestRunId = "";
        }
        public async Task CreateAuthClient()
        {
            await _azureAuthClient.GetAccessToken(_httpClient);
        }

        public async Task CreateSequential(TestRunInput testRunInput)
        {
            await CreateAuthClient();
            try
            {
                for(int i = 0; i< testRunInput.testData?.ToList().Count; i++)
                {
                    await CreateLoadTestRun(testRunInput.testData?.ToList()[i]!);
                    bool success = false;
                    do
                    {
                        var response = await GetLoadTestStatus();
                        var resObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(response)!;
                        string currStatus = resObj["status"]?.ToString()!;
                        success = currStatus == "DONE";
                        if (!success)
                        {
                            await Task.Delay(TimeSpan.FromMinutes(1));
                        }
                    } 
                    while (!success);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            // Add sequential load test run code here
        }

        private async Task<string> GetLoadTestStatus()
        {
            string apiUrl = $"https://{azureLoadTestResouceEndpoint}/test-runs/{TestRunId}?api-version=2022-11-01";
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
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

            try
            {
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
            catch(Exception ex)
            {
                Console.WriteLine($"Fail ho gya bhaiya {ex.ToString()}");
                throw ex;
            }
            
        }
    }
}
  