namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StartProcessResponse
    {
        public int ProcessId { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string Initiator { get; set; }
        public string Status { get; set; }
        public string CurrentStep { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

