using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;

namespace AzLoadTestWebAPI.Services
{
    public class AzureAuthClient
    {
        private string? accessToken;
        private async Task AcquireTokenAsync(HttpClient httpclient)
        {

            var clientId = "67f37fb4-955f-4254-976c-c1a20bf24607";
            var clientSecret = "FnL8Q~ydrWAyg~aAxCPdl9WyeVYM~9F3agT5nbvY";
            var authorityEndpoint = "https://login.microsoftonline.com/1e54ab8c-b863-4453-ba38-9339d2b282d1";

            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authorityEndpoint))
                .Build();

            var scopes = new[] { "https://cnt-prod.loadtesting.azure.com/.default" };
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            accessToken = result.AccessToken;

            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string?> GetAccessToken(HttpClient httpClient)
        {
            await AcquireTokenAsync(httpClient);
            return accessToken;
        }
    }
}
