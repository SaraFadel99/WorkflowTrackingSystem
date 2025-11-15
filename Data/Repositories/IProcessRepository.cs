using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Data.Repositories
{
    public interface IProcessRepository
    {
        Task<Process> CreateAsync(Process process);
        Task<Process?> GetByIdAsync(int id);
        Task<Process?> GetByIdWithWorkflowAsync(int id);
        Task UpdateAsync(Process process);
        Task<IEnumerable<Process>> GetProcessesAsync(int? workflowId = null, string? status = null, string? assignedTo = null);
       // Task<ProcessStepExecution> AddStepExecutionAsync(ProcessStepExecution stepExecution);
    }
}

