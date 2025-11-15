namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class ProcessListResponse
    {
        public int ProcessId { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string Initiator { get; set; }
        public string Status { get; set; }
        public string CurrentStep { get; set; }
        public string? AssignedTo { get; set; } // Who is assigned to the current step
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}

