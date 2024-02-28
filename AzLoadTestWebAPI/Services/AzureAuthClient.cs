using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;

namespace AzLoadTestWebAPI.Services
{
    public class AzureAuthClient
    {
        private string? LoadTestAccessToken;
        private string? MgmtAccessToken;
        private readonly IConfiguration _configuration;

        public AzureAuthClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private async Task<Tuple<string,string>> AcquireTokenAsync()
        {
            var clientId = _configuration.GetSection("AADAppReg").GetValue<string>("ClientId");
            var clientSecret = _configuration.GetSection("AADAppReg").GetValue<string>("ClientSecret");
            var authorityEndpoint = $"{_configuration.GetSection("AADAppReg").GetValue<string>("Authority")}/{_configuration.GetSection("AADAppReg").GetValue<string>("TenantId")}";

            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authorityEndpoint))
                .Build();

            var LoadTestScopes = new[] { _configuration.GetSection("AADAppReg").GetValue<string>("LoadTestScope") };
            var MgmtScopes = new[] { _configuration.GetSection("AADAppReg").GetValue<string>("MgmtScope") };

            var LoadTestResult = await app.AcquireTokenForClient(LoadTestScopes).ExecuteAsync();
            var MgmtResult = await app.AcquireTokenForClient(MgmtScopes).ExecuteAsync();

            LoadTestAccessToken = LoadTestResult.AccessToken;
            MgmtAccessToken = MgmtResult.AccessToken;

            return new Tuple<string,string>(LoadTestAccessToken, MgmtAccessToken);
        }

        

        public async Task<Tuple<string,string>> GetAccessToken()
        {
            return await AcquireTokenAsync();
        }
    }
}
