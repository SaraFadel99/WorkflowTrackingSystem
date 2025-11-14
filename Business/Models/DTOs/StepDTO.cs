namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StepDTO
    {
        public string StepName { get; set; }
        public string AssignedTo { get; set; }
        public string ActionType { get; set; }
        public string NextStep { get; set; }
    }
}
