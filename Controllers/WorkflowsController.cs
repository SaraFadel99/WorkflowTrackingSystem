using Microsoft.AspNetCore.Mvc;
using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Services;

namespace WorkflowTrackingSystem.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowsController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        public async Task<ActionResult<WorkflowResponse>> CreateWorkflow([FromBody] CreateWorkflowRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var workflow = await _workflowService.CreateWorkflowAsync(request);
                return Ok(workflow);
          
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the workflow.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkflowResponse>> UpdateWorkflow(int id, [FromBody] CreateWorkflowRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                WorkflowResponse workflow = await _workflowService.UpdateWorkflowAsync(id, request);
                return Ok(workflow);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the workflow.", error = ex.Message });
            }
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<WorkflowResponse>> GetWorkflowById(int id)
        //{
        //    var workflow = await _workflowService.GetWorkflowByIdAsync(id);
            
        //    if (workflow == null)
        //    {
        //        return NotFound(new { message = $"Workflow with id {id} not found." });
        //    }

        //    return Ok(workflow);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkflowResponse>>> GetAllWorkflows()
        {
            var workflows = await _workflowService.GetAllWorkflowsAsync();
            return Ok(workflows);
        }
    }
}

