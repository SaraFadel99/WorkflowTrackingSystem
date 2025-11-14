namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class CreateWorkflowRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StepDTO> Steps { get; set; }
    }
}
