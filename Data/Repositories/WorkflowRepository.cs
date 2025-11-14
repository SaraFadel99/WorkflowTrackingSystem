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

        public async Task<Workflow?> GetByIdAsync(int id)
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
    }
}

