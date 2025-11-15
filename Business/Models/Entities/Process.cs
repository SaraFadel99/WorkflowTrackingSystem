namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class Process
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Initiator { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; } // e.g., Active, Completed, Pending
        public string CurrentStep { get; set; } 
        public string NextStep { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
     
        public ICollection<ProcessStepExecution> StepExecutions { get; set; }
    }
}

