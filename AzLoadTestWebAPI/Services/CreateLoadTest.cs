namespace AzLoadTestWebAPI.Services
{
    public class CreateLoadTest
    {
        HttpClient _httpClient;
        AzureAuthClient _azureAuthClient;

        public CreateLoadTest(HttpClient httpClient, AzureAuthClient azureAuthClient) 
        {
            _httpClient = httpClient;
            _azureAuthClient = azureAuthClient;
        }
        public async Task CreateAuthClient()
        {
            await _azureAuthClient.GetAccessToken(_httpClient);
        }

        public async Task CreateSequential()
        {
            await CreateAuthClient();
            
            // Add sequential load test run code here
        }
    }
}
