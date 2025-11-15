namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class Process
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Initiator { get; set; }
        public string Status { get; set; } // e.g., "InProgress", "Completed", "Cancelled"
        public string CurrentStep { get; set; } // Current step name in the process
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        //should comment
       // public ICollection<ProcessStepExecution> StepExecutions { get; set; }
    }
}

