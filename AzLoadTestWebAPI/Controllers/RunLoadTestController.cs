using AzLoadTestWebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace AzLoadTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunLoadTestController : ControllerBase
    {
        [HttpPut]
        public string CreateLoadTestRuns([FromBody]TestRunInput testRunArray)
        {
            Console.WriteLine(testRunArray.ToString());
            return "Maze aa gy";
        }
    }
}
