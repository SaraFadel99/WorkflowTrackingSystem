namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class ProcessStepExecution
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public string StepName { get; set; }
        public string PerformedBy { get; set; }
        public string Action { get; set; } // e.g., "approve", "reject", null
        public string Status { get; set; } // e.g., "Completed", "Pending"
        public DateTime ExecutedDate { get; set; }
    }

}

