using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Models.Entities;
using WorkflowTrackingSystem.Data.Repositories;

namespace WorkflowTrackingSystem.Business.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IWorkflowRepository _workflowRepository;

        public ProcessService(IProcessRepository processRepository, IWorkflowRepository workflowRepository)
        {
            _processRepository = processRepository;
            _workflowRepository = workflowRepository;
        }

        public async Task<StartProcessResponse> StartProcessAsync(StartProcessRequest request)
        {
            // Validate workflow exists
            var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId);
            if (workflow == null)
            {
                throw new ArgumentException($"Workflow with ID {request.WorkflowId} not found.");
            }

            if (workflow.Steps == null || !workflow.Steps.Any())
            {
                throw new InvalidOperationException($"Workflow with ID {request.WorkflowId} has no steps defined.");
            }

            // Get the first step (assuming steps are ordered, or we take the first one)
            var firstStep = workflow.Steps.OrderBy(s => s.Id).FirstOrDefault();
            if (firstStep == null)
            {
                throw new InvalidOperationException("Cannot determine the first step of the workflow.");
            }

            // Create process
            var process = new Process
            {
                WorkflowId = request.WorkflowId,
                Initiator = request.Initiator,
                Status = "InProgress",
                CurrentStep = firstStep.StepName,
                CreatedDate = DateTime.UtcNow,
             //   StepExecutions = new List<ProcessStepExecution>()
            };

            var createdProcess = await _processRepository.CreateAsync(process);

            // Map to response
            return new StartProcessResponse
            {
                ProcessId = createdProcess.Id,
                WorkflowId = createdProcess.WorkflowId,
                WorkflowName = workflow.Name,
                Initiator = createdProcess.Initiator,
                Status = createdProcess.Status,
                CurrentStep = createdProcess.CurrentStep,
                CreatedDate = createdProcess.CreatedDate
            };
        }

        public async Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request)
        {
            // Get process with workflow information
            var process = await _processRepository.GetByIdAsync(request.ProcessId);
            if (process == null)
            {
                throw new ArgumentException($"Process with ID {request.ProcessId} not found.");
            }

            // Get workflow to validate step
            var workflow = await _workflowRepository.GetByIdAsync(process.WorkflowId);
            if (workflow == null)
            {
                throw new InvalidOperationException($"Workflow for process {request.ProcessId} not found.");
            }

            // Validate current step matches
            if (process.CurrentStep != request.StepName)
            {
                throw new InvalidOperationException(
                    $"Cannot execute step '{request.StepName}'. Current step is '{process.CurrentStep}'.");
            }

            // Find the workflow step
            var workflowStep = workflow.Steps?.FirstOrDefault(s => s.StepName == request.StepName);
            if (workflowStep == null)
            {
                throw new ArgumentException($"Step '{request.StepName}' not found in workflow.");
            }

            // Validate user has permission (check if performed_by matches assigned_to role)
            // Note: This is a simple check - you might want more sophisticated role-based validation
            if (!string.IsNullOrEmpty(workflowStep.AssignedTo) && 
                !request.PerformedBy.Equals(workflowStep.AssignedTo, StringComparison.OrdinalIgnoreCase))
            {
                // Allow if it's a role match (e.g., "manager" role)
                // For now, we'll allow it but you can add stricter validation
            }

            // Validate action type
            if (workflowStep.ActionType == "approve_reject" && 
                !new[] { "approve", "reject" }.Contains(request.Action.ToLower()))
            {
                throw new ArgumentException(
                    $"Action '{request.Action}' is not valid for step '{request.StepName}'. Expected 'approve' or 'reject'.");
            }

            // Create step execution record
            //var stepExecution = new ProcessStepExecution
            //{
            //    ProcessId = process.Id,
            //    StepName = request.StepName,
            //    PerformedBy = request.PerformedBy,
            //    Action = request.Action,
            //    Status = "Completed",
            //    ExecutedDate = DateTime.UtcNow
            //};

            //await _processRepository.AddStepExecutionAsync(stepExecution);

            // Determine next step
            string? nextStep = null;
            string processStatus = "InProgress";

            if (request.Action.ToLower() == "reject")
            {
                // If rejected, process might be cancelled or go to a rejection step
                processStatus = "Rejected";
                process.CompletedDate = DateTime.UtcNow;
            }
            else if (!string.IsNullOrEmpty(workflowStep.NextStep) && 
                     workflowStep.NextStep != "Completed")
            {
                nextStep = workflowStep.NextStep;
                process.CurrentStep = nextStep;
            }
            else
            {
                // Process completed
                processStatus = "Completed";
                process.CurrentStep = "Completed";
                process.CompletedDate = DateTime.UtcNow;
            }

            process.Status = processStatus;
            await _processRepository.UpdateAsync(process);

            // Map to response
            return new ExecuteStepResponse
            {
                ProcessId = process.Id,
                StepName = request.StepName,
                PerformedBy = request.PerformedBy,
                Action = request.Action,
                Status = "Completed",
                NextStep = nextStep,
                ProcessStatus = processStatus,
                ExecutedDate = DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<ProcessListResponse>> GetProcessesAsync(int? workflowId = null, string? status = null, string? assignedTo = null)
        {
            // Get filtered processes from repository
            var processes = await _processRepository.GetProcessesAsync(workflowId, status, assignedTo);

            if (!processes.Any())
            {
                return new List<ProcessListResponse>();
            }

            // Get unique workflow IDs
            var workflowIds = processes.Select(p => p.WorkflowId).Distinct().ToList();

            // Load all workflows with their steps
            var workflows = new Dictionary<int, Workflow>();
            foreach (var workFlowid in workflowIds)
            {
                var workflow = await _workflowRepository.GetByIdAsync(workFlowid);
                if (workflow != null)
                {
                    workflows[workFlowid] = workflow;
                }
            }

            // Map processes to response DTOs
            var response = new List<ProcessListResponse>();
            foreach (var process in processes)
            {
                if (!workflows.TryGetValue(process.WorkflowId, out var workflow))
                {
                    continue; // Skip if workflow not found
                }

                // Find the current step in the workflow to get AssignedTo
                string? assignedToValue = null;
                if (!string.IsNullOrEmpty(process.CurrentStep) && workflow.Steps != null)
                {
                    var currentWorkflowStep = workflow.Steps.FirstOrDefault(ws => ws.StepName == process.CurrentStep);
                    assignedToValue = currentWorkflowStep?.AssignedTo;
                }

                response.Add(new ProcessListResponse
                {
                    ProcessId = process.Id,
                    WorkflowId = process.WorkflowId,
                    WorkflowName = workflow.Name,
                    Initiator = process.Initiator,
                    Status = process.Status,
                    CurrentStep = process.CurrentStep,
                    AssignedTo = assignedToValue,
                    CreatedDate = process.CreatedDate,
                    CompletedDate = process.CompletedDate
                });
            }

            return response;
        }
    }
}

