using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileProcessingController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] IFormFile? file)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid file" });
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension == ".csv")
            {
                return Ok("csv");
            }
            else if (extension == ".json")
            {
                return Ok("json");
            }
            else
            {
                return BadRequest(new ProblemDetails { Title = "Unsupported file format" });
            }

        }
    }
}
