namespace WorkflowTrackingSystem.Business.Models.Entities
{
    public class ProcessStepExecution
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public string StepName { get; set; }
        public string PerformedBy { get; set; }
        public string Action { get; set; } 
        public string Status { get; set; } 
        public DateTime ExecutedDate { get; set; }
        public bool? ValidationPassed { get; set; }
        public string ValidationMessage { get; set; }
    }

}

