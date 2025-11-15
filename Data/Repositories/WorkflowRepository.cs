using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Data.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkflowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Workflow> CreateAsync(Workflow workflow)
        {
            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync();
            return workflow;
        }

        public async Task<Workflow> GetByIdAsync(int id)
        {
            return await _context.Workflows
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workflow>> GetAllAsync()
        {
            return await _context.Workflows
                .Include(w => w.Steps)
                .ToListAsync();
        }

        public async Task<Workflow> UpdateAsync(Workflow workflow)
        {
            // Get existing workflow with steps
            Workflow existingWorkflow = await _context.Workflows
                .Include(w => w.Steps)
                .FirstOrDefaultAsync(w => w.Id == workflow.Id);

            if (existingWorkflow == null)
            {
                throw new ArgumentException($"Workflow with ID {workflow.Id} not found.");
            }

            // Update workflow properties
            existingWorkflow.Name = workflow.Name;
            existingWorkflow.Description = workflow.Description;

            // Remove existing steps
            _context.WorkflowSteps.RemoveRange(existingWorkflow.Steps);

            // Add new steps from the workflow parameter
            if (workflow.Steps != null && workflow.Steps.Any())
            {
                foreach (var step in workflow.Steps)
                {
                    // Ensure WorkflowId is set
                    step.WorkflowId = existingWorkflow.Id;
                    _context.WorkflowSteps.Add(step);
                }
            }

            await _context.SaveChangesAsync();

            // Reload the workflow with updated steps
            return workflow;
        }
    }
}

