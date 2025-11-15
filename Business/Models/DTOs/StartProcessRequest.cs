using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StartProcessRequest
    {
        [Required(ErrorMessage = "Workflow ID is required.")]
        [JsonPropertyName("workflow_id")]
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "Initiator is required.")]
        [StringLength(100, ErrorMessage = "Initiator cannot exceed 100 characters.")]
        [JsonPropertyName("initiator")]
        public string Initiator { get; set; }
    }
}

