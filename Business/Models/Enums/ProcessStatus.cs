using System.Text.Json.Serialization;

namespace WorkflowTrackingSystem.Business.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProcessStatus
    {
        Active,
        Completed,
        Pending
    }
}

