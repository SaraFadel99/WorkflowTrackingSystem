using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StepDTO
    {
        [Required(ErrorMessage = "Step name is required.")]
        [StringLength(50, ErrorMessage = "Step name cannot exceed 50 characters.")]
        [JsonPropertyName("step_name")]
        public string StepName { get; set; }

        [Required(ErrorMessage = "Assigned to is required.")]
        [StringLength(100, ErrorMessage = "Assigned to cannot exceed 100 characters.")]
        [JsonPropertyName("assigned_to")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "Action type is required.")]
        [StringLength(50, ErrorMessage = "Action type cannot exceed 50 characters.")]
        [JsonPropertyName("action_type")]
        public string ActionType { get; set; }

        [StringLength(50, ErrorMessage = "Next step cannot exceed 50 characters.")]
        [JsonPropertyName("next_step")]
        public string NextStep { get; set; }

        public bool RequireValidation { get; set; }
        [StringLength(50, ErrorMessage = "Next step cannot exceed 50 characters.")]
        public string ValidationAPIURL { get; set; }
    }
}
