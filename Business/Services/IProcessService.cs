using WorkflowTrackingSystem.Business.Models.DTOs;

namespace WorkflowTrackingSystem.Business.Services
{
    public interface IProcessService
    {
        Task<StartProcessResponse> StartProcessAsync(StartProcessRequest request);
        Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request);
        Task<IEnumerable<ProcessListResponse>> GetProcessesAsync(int? workflowId = null, string? status = null, string? assignedTo = null);
    }
}

