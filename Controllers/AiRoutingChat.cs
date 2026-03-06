using Assignment.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiRoutingChat : ControllerBase
    {
        private readonly IAiRoutingService _aiRoutingService;
        public AiRoutingChat(IAiRoutingService aiRoutingService)
        {
            _aiRoutingService = aiRoutingService;
        }

        [HttpPost("{conversation:int}")]
        public async Task<IActionResult> Post([FromBody] string request,[FromRoute] int conversation)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return BadRequest("Message cannot be empty.");
            }
            var departmentInfo = await _aiRoutingService.DetectDepartment(request, conversation);
            return Ok(departmentInfo);
        }
    }
}
