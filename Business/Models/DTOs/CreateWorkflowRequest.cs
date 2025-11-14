using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class CreateWorkflowRequest
    {
        [Required(ErrorMessage = "Workflow name is required.")]
        [StringLength(200, ErrorMessage = "Workflow name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Workflow must have at least one step.")]
        [MinLength(1, ErrorMessage = "Workflow must have at least one step.")]
        public ICollection<StepDTO> Steps { get; set; }
    }
}
