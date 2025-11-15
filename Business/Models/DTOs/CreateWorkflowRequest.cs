using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class CreateWorkflowRequest
    {
        [Required(ErrorMessage = "Workflow name is required.")]
        [StringLength(50, ErrorMessage = "Workflow name cannot exceed 50 characters.")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Workflow must have at least one step.")]
        [MinLength(1, ErrorMessage = "Workflow must have at least one step.")]
        [JsonPropertyName("steps")]
        public ICollection<StepDTO> Steps { get; set; }
    }
}
