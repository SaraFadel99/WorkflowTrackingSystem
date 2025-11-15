using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Business.Models.DTOs
{
    public class StartProcessRequest
    {
        [Required(ErrorMessage = "Workflow ID is required.")]
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "Initiator is required.")]
        [StringLength(100, ErrorMessage = "Initiator cannot exceed 100 characters.")]
        public string Initiator { get; set; }
    }
}

