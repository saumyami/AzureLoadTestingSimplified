using AzLoadTestWebAPI.Model;
using AzLoadTestWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzLoadTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunLoadTestController : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> CreateLoadTestRuns([FromBody]TestRunInput testRunInput, RunLoadTests createLoadTest)
        {
            Console.WriteLine(testRunInput.ToString());
            var testDataOutputList = await createLoadTest.CreateSequential(testRunInput);
            var testRunOutput = new TestRunOutput(testRunInput);
            testRunOutput.loadTestRuns = testDataOutputList;
            return Ok(testRunOutput);
        }
    }
}
