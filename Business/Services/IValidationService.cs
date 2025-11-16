using WorkflowTrackingSystem.Business.Models.Entities;
using WorkflowTrackingSystem.Business.Models.DTOs;

namespace WorkflowTrackingSystem.Business.Services
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateStepAsync(ExecuteStepRequest request, string ValidationAPIURL);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
