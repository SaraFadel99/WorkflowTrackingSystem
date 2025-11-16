using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Models.Entities;

namespace WorkflowTrackingSystem.Business.Services
{

    public class ValidationService : IValidationService
    {
        private readonly ILogger<ValidationService> _logger;
        //private readonly HttpClient _httpClient;

        public ValidationService(ILogger<ValidationService> logger/*, IHttpClientFactory httpClientFactory*/)
        {
            _logger = logger;
           // _httpClient = httpClientFactory.CreateClient(); 
        }

        public async Task<ValidationResult> ValidateStepAsync( ExecuteStepRequest request, string ValidationAPIURL)
        {
            // Simulate calling an external API for validation.
            // Replace this block with an actual HttpClient call if needed.
            try
            {
                await Task.Delay(150); // simulate network latency
                // Default success
                return new ValidationResult { IsValid = true, Message = "Validation succeeded." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation call failed for process {ProcessId}, step {StepName}.", request.ProcessId, request.StepName);
                return new ValidationResult { IsValid = false, Message = $"Validation service error: {ex.Message}" };
            }
        }
    }
}
