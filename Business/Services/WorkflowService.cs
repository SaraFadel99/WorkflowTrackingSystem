using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Models.Entities;
using WorkflowTrackingSystem.Data.Repositories;

namespace WorkflowTrackingSystem.Business.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public async Task<WorkflowResponse> CreateWorkflowAsync(CreateWorkflowRequest request)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Workflow name is required.", nameof(request));
            }

            if (request.Steps == null || !request.Steps.Any())
            {
                throw new ArgumentException("Workflow must have at least one step.", nameof(request));
            }

            // Map DTO to Entity
            Workflow workflow = new Workflow
            {
                Name = request.Name,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                Steps = request.Steps.Select(stepDto => new WorkflowStep
                {
                    StepName = stepDto.StepName,
                    AssignedTo = stepDto.AssignedTo,
                    ActionType = stepDto.ActionType,
                    NextStep = stepDto.NextStep,
                    RequireValidation = stepDto.RequireValidation,
                    ValidationAPIURL = stepDto.ValidationAPIURL
                }).ToList()
            };

            // Save to database
            Workflow createdWorkflow = await _workflowRepository.CreateAsync(workflow);

            // Map Entity to Response DTO
            return MapToResponse(createdWorkflow);
        }

        //public async Task<WorkflowResponse> GetWorkflowByIdAsync(int id)
        //{
        //    var workflow = await _workflowRepository.GetByIdAsync(id);
        //    return workflow != null ? MapToResponse(workflow) : null;
        //}

        public async Task<IEnumerable<WorkflowResponse>> GetAllWorkflowsAsync()
        {
            var workflows = await _workflowRepository.GetAllAsync();
      
            return workflows.Select(MapToResponse);
        }

        public async Task<WorkflowResponse> UpdateWorkflowAsync(int id, CreateWorkflowRequest request)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Workflow name is required.", nameof(request));
            }

            if (request.Steps == null || !request.Steps.Any())
            {
                throw new ArgumentException("Workflow must have at least one step.", nameof(request));
            }

            // Map DTO to Entity
            Workflow workflow = new Workflow
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
              //  CreatedDate = existingWorkflow.CreatedDate, 
                Steps = request.Steps.Select(stepDto => new WorkflowStep
                {
                    StepName = stepDto.StepName,
                    AssignedTo = stepDto.AssignedTo,
                    ActionType = stepDto.ActionType,
                    NextStep = stepDto.NextStep
                }).ToList()
            };

            // Update in database
            Workflow updatedWorkflow = await _workflowRepository.UpdateAsync(workflow);

            // Map Entity to Response DTO
            return MapToResponse(updatedWorkflow);
        }

        private static WorkflowResponse MapToResponse(Workflow workflow)
        {
            return new WorkflowResponse
            {
                Id = workflow.Id,
                Name = workflow.Name,
                Description = workflow.Description,
                Steps = workflow.Steps?.Select(step => new StepDTO
                {
                    StepName = step.StepName,
                    AssignedTo = step.AssignedTo,
                    ActionType = step.ActionType,
                    NextStep = step.NextStep,
                    RequireValidation = step.RequireValidation,
                    ValidationAPIURL = step.ValidationAPIURL
                }).ToList() ?? new List<StepDTO>()
            };
        }
    }
}

