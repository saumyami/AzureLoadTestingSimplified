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
        public async Task<string> CreateLoadTestRuns([FromBody]TestRunInput testRunArray, CreateLoadTest createLoadTest)
        {
            Console.WriteLine(testRunArray.ToString());
            await createLoadTest.CreateSequential();
            return "Maze aa gy";
        }
    }
}
