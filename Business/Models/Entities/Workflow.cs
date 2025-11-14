namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class CreateWorkflowRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkflowStep> Steps { get; set; }
        public int CreatedDate { get; set; }
    }
}
