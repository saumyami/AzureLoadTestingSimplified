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
        public async Task<string> CreateLoadTestRuns([FromBody]TestRunInput testRunInput, RunLoadTests createLoadTest)
        {
            Console.WriteLine(testRunInput.ToString());
            await createLoadTest.CreateSequential(testRunInput);
            return "Maze aa gy";
        }
    }
}
