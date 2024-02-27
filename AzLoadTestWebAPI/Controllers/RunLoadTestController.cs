using AzLoadTestWebAPI.Model;
using AzLoadTestWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzLoadTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunLoadTestController : ControllerBase
    {
        [HttpPut]
        public async Task<string> CreateLoadTestRuns([FromBody]TestRunInput testRunArray, AzureAuthClient authClient, HttpClient httpClient)
        {
            Console.WriteLine(testRunArray.ToString());
            var accessToken = await authClient.GetAccessToken(httpClient);
            return "Maze aa gy";
        }
    }
}
