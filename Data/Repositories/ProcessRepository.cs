using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Data.Repositories
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly ApplicationDbContext _context;

        public ProcessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Process> CreateAsync(Process process)
        {
            _context.Processes.Add(process);
            await _context.SaveChangesAsync();
            return process;
        }

        public async Task<Process?> GetByIdAsync(int id)
        {
            return await _context.Processes
                //.Include(p => p.StepExecutions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Process?> GetByIdWithWorkflowAsync(int id)
        {
            return await _context.Processes
                //.Include(p => p.StepExecutions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Process process)
        {
            _context.Processes.Update(process);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Process>> GetProcessesAsync(int? workflowId = null, string? status = null, string? assignedTo = null)
        {
            var query = _context.Processes.AsQueryable();

            // Filter by workflow_id
            if (workflowId.HasValue)
            {
                query = query.Where(p => p.WorkflowId == workflowId.Value);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(status))
            {
                // Map status values: "Active" -> "InProgress", "Completed" -> "Completed", "Pending" -> "InProgress" or check actual status
                string statusFilter = status.ToLower() switch
                {
                    "active" => "InProgress",
                    "completed" => "Completed",
                    "pending" => "InProgress",
                    "rejected" => "Rejected",
                    _ => status // Use the provided status as-is if it doesn't match standard values
                };
                query = query.Where(p => p.Status == statusFilter);
            }

            // Filter by assigned_to - requires joining with WorkflowSteps
            if (!string.IsNullOrEmpty(assignedTo))
            {
                // Join with WorkflowSteps to find processes where current step is assigned to the specified user
                query = query.Where(p =>
                    _context.WorkflowSteps
                        .Any(ws => ws.WorkflowId == p.WorkflowId
                            && ws.StepName == p.CurrentStep
                            && ws.AssignedTo.Equals(assignedTo, StringComparison.OrdinalIgnoreCase)));
            }

            return await query
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        //public async Task<ProcessStepExecution> AddStepExecutionAsync(ProcessStepExecution stepExecution)
        //{
        //    _context.ProcessStepExecutions.Add(stepExecution);
        //    await _context.SaveChangesAsync();
        //    return stepExecution;
        //}
    }
}

