using WorkflowTrackingSystem.Business.Models.DTOs;
using WorkflowTrackingSystem.Business.Models.Entities;
using WorkflowTrackingSystem.Business.Models.Enums;
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
            Workflow workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId);
            if (workflow == null)
            {
                throw new ArgumentException($"Workflow with ID {request.WorkflowId} not found.");
            }

            if (workflow.Steps == null || !workflow.Steps.Any())
            {
                throw new InvalidOperationException($"Workflow with ID {request.WorkflowId} has no steps defined.");
            }

            // Get the first step (assuming steps are ordered, or we take the first one)
            var orderedSteps = workflow.Steps.OrderBy(s => s.Id);
            WorkflowStep firstStep = orderedSteps.FirstOrDefault();
            if (firstStep == null)
            {
                throw new InvalidOperationException("Cannot determine the first step of the workflow.");
            }

            // Create process
            Process process = new Process
            {
                WorkflowId = request.WorkflowId,
                Initiator = request.Initiator,
                Status = ProcessStatusToString(ProcessStatus.Active), // Start as Active
                CurrentStep = firstStep.StepName,
                NextStep = firstStep.NextStep,
                CreatedDate = DateTime.UtcNow,
                StepExecutions = new List<ProcessStepExecution>()
            };

            Process createdProcess = await _processRepository.CreateAsync(process);

            // Map to response
            return new StartProcessResponse
            {
                ProcessId = createdProcess.Id,
                WorkflowId = createdProcess.WorkflowId,
                WorkflowName = workflow.Name,
                Initiator = createdProcess.Initiator,
                Status = StringToProcessStatus(createdProcess.Status),
                CurrentStep = createdProcess.CurrentStep,
                NextStep = createdProcess.NextStep,
                CreatedDate = createdProcess.CreatedDate
            };
        }

        public async Task<ExecuteStepResponse> ExecuteStepAsync(ExecuteStepRequest request)
        {
            // Get process with workflow information
            Process process = await _processRepository.GetByIdAsync(request.ProcessId);
            if (process == null)
            {
                throw new ArgumentException($"Process with ID {request.ProcessId} not found.");
            }

            // Get workflow to validate step
            Workflow workflow = await _workflowRepository.GetByIdAsync(process.WorkflowId);
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
            WorkflowStep workflowStep = workflow.Steps?.FirstOrDefault(s => s.StepName == request.StepName);
            if (workflowStep == null)
            {
                throw new ArgumentException($"Step '{request.StepName}' not found in workflow.");
            }

            // Validate action type - enum ensures only valid values
            if (workflowStep.ActionType == "approve_reject" && 
                request.Action != StepAction.approve && request.Action != StepAction.reject)
            {
                throw new ArgumentException(
                    $"Action '{request.Action}' is not valid for step '{request.StepName}'. Expected 'approve' or 'reject'.");
            }

            // Create step execution record
            ProcessStepExecution stepExecution = new ProcessStepExecution
            {
                ProcessId = process.Id,
                StepName = request.StepName,
                PerformedBy = request.PerformedBy,
                Action = StepActionToString(request.Action),
                Status = "Completed",
                ExecutedDate = DateTime.UtcNow
            };

            await _processRepository.AddStepExecutionAsync(stepExecution);

            // Determine next step
            string nextStep = null;
            ProcessStatus processStatus = ProcessStatus.Active;

            if (request.Action == StepAction.reject)
            {
                // If rejected, mark as Completed (terminal state)
                processStatus = ProcessStatus.Completed;
                process.CompletedDate = DateTime.UtcNow;
            }
            else if (!string.IsNullOrEmpty(workflowStep.NextStep) && 
                     workflowStep.NextStep != "Completed")
            {
                nextStep = workflowStep.NextStep;
                process.CurrentStep = nextStep;
                processStatus = ProcessStatus.Active; // Still active, moving to next step
            }
            else
            {
                // Process completed
                processStatus = ProcessStatus.Completed;
                process.CurrentStep = "Completed";
                process.CompletedDate = DateTime.UtcNow;
            }

            process.Status = ProcessStatusToString(processStatus);
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
                    Status = StringToProcessStatus(process.Status),
                    CurrentStep = process.CurrentStep,
                    AssignedTo = assignedToValue,
                    CreatedDate = process.CreatedDate,
                    CompletedDate = process.CompletedDate
                });
            }

            return response;
        }

        // Helper methods for enum/string conversion
        private static string ProcessStatusToString(ProcessStatus status)
        {
            return status switch
            {
                ProcessStatus.Active => "Active",
                ProcessStatus.Completed => "Completed",
                ProcessStatus.Pending => "Pending",
                _ => status.ToString()
            };
        }

        private static ProcessStatus StringToProcessStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                return ProcessStatus.Pending;

            return status.ToLower() switch
            {
                "active" or "inprogress" => ProcessStatus.Active,
                "completed" or "rejected" => ProcessStatus.Completed, // Map Rejected to Completed
                "pending" => ProcessStatus.Pending,
                _ => ProcessStatus.Pending // Default to Pending for unknown values
            };
        }

        private static string StepActionToString(StepAction action)
        {
            return action.ToString().ToLower(); // "approve" or "reject"
        }

        private static StepAction StringToStepAction(string action)
        {
            if (string.IsNullOrEmpty(action))
                throw new ArgumentException("Action cannot be null or empty.");

            return action.ToLower() switch
            {
                "approve" => StepAction.approve,
                "reject" => StepAction.reject,
                _ => throw new ArgumentException($"Invalid action value: {action}. Valid values are: approve, reject")
            };
        }
    }
}

