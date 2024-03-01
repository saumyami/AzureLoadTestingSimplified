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

    [Route("api/[controller]")]
    [ApiController]
    public class RunSingleLoadTestController : ControllerBase
    {
        [HttpPut]
        public async Task<IActionResult> CreateLoadTestRuns([FromBody] TestRunData testRundata, [FromHeader] string subscriptionId, [FromHeader] string resourceGroupName, [FromHeader] string azureLoadTestingResourceName, RunLoadTests createLoadTest)
        {
            Console.WriteLine(testRundata.ToString());
            await createLoadTest.CreateAuthClient();
            createLoadTest.azureLoadTestResouceEndpoint = await createLoadTest.GetAzureLoadTestingDataPlaneEndpoint(subscriptionId, resourceGroupName, azureLoadTestingResourceName);
            var testDataOutput = await createLoadTest.CreateSequentialSingleRun(testRundata);
            return Ok(testDataOutput);
        }
    }

    
}
