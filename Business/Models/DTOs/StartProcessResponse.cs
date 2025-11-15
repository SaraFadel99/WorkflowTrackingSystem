using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StartProcessResponse
    {
        [JsonPropertyName("process_id")]
        public int ProcessId { get; set; }

        [JsonPropertyName("workflow_id")]
        public int WorkflowId { get; set; }

        [JsonPropertyName("workflow_name")]
        public string WorkflowName { get; set; }

        [JsonPropertyName("initiator")]
        public string Initiator { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("current_step")]
        public string CurrentStep { get; set; }
        public string NextStep { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}

