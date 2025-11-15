namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class ExecuteStepResponse
    {
        public int ProcessId { get; set; }
        public string StepName { get; set; }
        public string PerformedBy { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string? NextStep { get; set; }
        public string ProcessStatus { get; set; }
        public DateTime ExecutedDate { get; set; }
    }
}

