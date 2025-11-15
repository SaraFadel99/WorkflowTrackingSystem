using Microsoft.AspNetCore.Mvc;
using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Services;

namespace WorkflowTrackingSystem.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _processService;

        public ProcessesController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpPost("start")]
        public async Task<ActionResult<StartProcessResponse>> StartProcess([FromBody] StartProcessRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                StartProcessResponse response = await _processService.StartProcessAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while starting the process.", error = ex.Message });
            }
        }

        [HttpPost("execute")]
        public async Task<ActionResult<ExecuteStepResponse>> ExecuteStep([FromBody] ExecuteStepRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _processService.ExecuteStepAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while executing the step.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessListResponse>>> GetProcesses(
            [FromQuery] int? workflow_id = null,
            [FromQuery] string status = null,
            [FromQuery] string assigned_to = null)
        {
            try
            {
                var processes = await _processService.GetProcessesAsync(workflow_id, status, assigned_to);
                return Ok(processes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving processes.", error = ex.Message });
            }
        }

  
    }
}

