using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StepDTO
    {
        [Required(ErrorMessage = "Step name is required.")]
        [StringLength(200, ErrorMessage = "Step name cannot exceed 200 characters.")]
        public string StepName { get; set; }

        [Required(ErrorMessage = "Assigned to is required.")]
        [StringLength(100, ErrorMessage = "Assigned to cannot exceed 100 characters.")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "Action type is required.")]
        [StringLength(50, ErrorMessage = "Action type cannot exceed 50 characters.")]
        public string ActionType { get; set; }

        [StringLength(200, ErrorMessage = "Next step cannot exceed 200 characters.")]
        public string NextStep { get; set; }
    }
}
