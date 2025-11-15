using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Data.Repositories
{
    public interface IWorkflowRepository
    {
        Task<Workflow> CreateAsync(Workflow workflow);
        Task<Workflow?> GetByIdAsync(int id);
        Task<IEnumerable<Workflow>> GetAllAsync();
        Task<Workflow> UpdateAsync(Workflow workflow);
    }
}

