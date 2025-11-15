using WorkflowTrackingSystem.Business.Models.DTOs;

namespace WorkflowTrackingSystem.Business.Services
{
    public interface IWorkflowService
    {
        Task<WorkflowResponse> CreateWorkflowAsync(CreateWorkflowRequest request);
   //     Task<WorkflowResponse?> GetWorkflowByIdAsync(int id);
        Task<IEnumerable<WorkflowResponse>> GetAllWorkflowsAsync();
        Task<WorkflowResponse> UpdateWorkflowAsync(int id, CreateWorkflowRequest request);
    }
}

