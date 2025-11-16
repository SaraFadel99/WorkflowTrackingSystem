namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class WorkflowStep
    {
        public int Id { get; set; }
        public string StepName { get; set; }
        public string AssignedTo { get; set; }
        public string ActionType { get; set; }
        public string NextStep { get; set; }
        public int WorkflowId { get; set; }
        public bool RequireValidation { get; set; }
        public string ValidationAPIURL { get; set; }
    }
}
