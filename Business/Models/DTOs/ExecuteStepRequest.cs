using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class ExecuteStepRequest
    {
        [Required(ErrorMessage = "Process ID is required.")]
        [JsonPropertyName("process_id")]
        public int ProcessId { get; set; }

        [Required(ErrorMessage = "Step name is required.")]
        [StringLength(200, ErrorMessage = "Step name cannot exceed 200 characters.")]
        [JsonPropertyName("step_name")]
        public string StepName { get; set; }

        [Required(ErrorMessage = "Performed by is required.")]
        [StringLength(100, ErrorMessage = "Performed by cannot exceed 100 characters.")]
        [JsonPropertyName("performed_by")]
        public string PerformedBy { get; set; }

        [Required(ErrorMessage = "Action is required.")]
        [StringLength(50, ErrorMessage = "Action cannot exceed 50 characters.")]
        [JsonPropertyName("action")]
        public string Action { get; set; } // e.g., "approve", "reject", "input"
    }
}

