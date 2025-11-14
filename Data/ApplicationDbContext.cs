using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Workflow entity
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedDate).IsRequired();
            });

            // Configure WorkflowStep entity
            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StepName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AssignedTo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ActionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.NextStep).HasMaxLength(200);

                // Configure relationship (one-way navigation as per user preference)
                entity.HasOne<Workflow>()
                    .WithMany(w => w.Steps)
                    .HasForeignKey(ws => ws.WorkflowId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

