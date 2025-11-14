namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class WorkflowResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StepDTO> Steps { get; set; }
    }
}
