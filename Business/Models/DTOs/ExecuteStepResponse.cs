using System.Text.Json.Serialization;
using WorkflowTrackingSystem.Business.Models.Enums;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class ExecuteStepResponse
    {
        [JsonPropertyName("process_id")]
        public int ProcessId { get; set; }

        [JsonPropertyName("step_name")]
        public string StepName { get; set; }

        [JsonPropertyName("performed_by")]
        public string PerformedBy { get; set; }

        [JsonPropertyName("action")]
        public StepAction Action { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("next_step")]
        public string? NextStep { get; set; }

        [JsonPropertyName("process_status")]
        public ProcessStatus ProcessStatus { get; set; }

        [JsonPropertyName("executed_date")]
        public DateTime ExecutedDate { get; set; }
    }
}

