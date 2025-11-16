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
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessStepExecution> ProcessStepExecutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Workflow entity
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(300);
                entity.Property(e => e.CreatedDate).IsRequired();
            });

            // Configure WorkflowStep entity
            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StepName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AssignedTo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ActionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.NextStep).HasMaxLength(50);
                entity.Property(e => e.RequireValidation);
                entity.Property(e => e.ValidationAPIURL).HasMaxLength(50);

                entity.HasOne<Workflow>()
                    .WithMany(w => w.Steps)
                    .HasForeignKey(ws => ws.WorkflowId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Process entity
            modelBuilder.Entity<Process>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkflowId).IsRequired();
                entity.Property(e => e.Initiator).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CurrentStep).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).IsRequired();

                // Configure relationship to Workflow
                entity.HasOne<Workflow>()
                    .WithMany()
                    .HasForeignKey(p => p.WorkflowId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ProcessStepExecution entity
            modelBuilder.Entity<ProcessStepExecution>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProcessId).IsRequired();
                entity.Property(e => e.StepName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PerformedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ExecutedDate).IsRequired();

                // Configure relationship to Process
                entity.HasOne<Process>()
                    .WithMany(p => p.StepExecutions)
                    .HasForeignKey(pse => pse.ProcessId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

