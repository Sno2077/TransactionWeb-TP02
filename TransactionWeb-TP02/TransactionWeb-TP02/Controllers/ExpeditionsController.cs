using Microsoft.AspNetCore.Mvc;
using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Models;
using TransactionWeb_TP02.Services;

namespace TransactionWeb_TP02.Controllers
{
    [ApiController]
    [Route("api/expeditions")]
    public class ExpeditionController : ControllerBase
    {
        private readonly ExpeditionMemory _expeditionMemory;
        private readonly AssignmentMemory _assignmentMemory;
        private readonly ExpeditionWorkflowService _workflow;
        private readonly AssignmentService _assignmentService;
        
        public ExpeditionController(
            ExpeditionMemory expeditionMemory,
            AssignmentMemory assignmentMemory,
            ExpeditionWorkflowService workflow,
            AssignmentService assignmentService)
        {
            _expeditionMemory = expeditionMemory;
            _assignmentMemory = assignmentMemory;
            _workflow = workflow;
            _assignmentService = assignmentService;
        }

        //===== GET =====
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_expeditionMemory.GetExpeditions());
        }

        [HttpGet("{id:int}/assignments")]
        public IActionResult GetAssignments(int id)
        {
            if (!_expeditionMemory.GetExpeditions().Any(e => e.Id == id))
                return Problem(detail: "Expedition not found.", statusCode: 404);

            return Ok(_assignmentMemory.GetAssignments().FindAll(a => a.ExpeditionId == id));
        }

        //===== POST : workflow =====
        [HttpPost("{id:int}/cancel")]
        public IActionResult Cancel(int id)
        {
            WorkflowResult result = _workflow.Cancel(id);

            switch (result)
            {
                case WorkflowResult.Success:
                    return Ok(_expeditionMemory.GetExpeditions().Find(e => e.Id == id));
                case WorkflowResult.NotFound:
                    return Problem(detail: "Expedition not found.", statusCode: 404);
                default:
                    return Problem(detail: "Only a planned or open expedition can be cancelled.", statusCode: 409);
            }
        }

        //===== POST : opération transactionnelle =====
        [HttpPost("{id:int}/assignments")]
        public IActionResult Assign(int id, [FromBody] AssignmentDTO dto, [FromQuery] bool simulateFailure = false)
        {
            AssignOutcome outcome = _assignmentService.Assign(dto.PirateId, id, simulateFailure, out Assignment? assignment);

            switch (outcome)
            {
                case AssignOutcome.Success:
                    return Created($"/api/expeditions/{id}/assignments", assignment);
                case AssignOutcome.PirateNotFound:
                    return Problem(detail: "Pirate not found.", statusCode: 404);
                case AssignOutcome.ExpeditionNotFound:
                    return Problem(detail: "Expedition not found.", statusCode: 404);
                case AssignOutcome.ExpeditionNotOpen:
                    return Problem(detail: "Expedition is not open.", statusCode: 409);
                case AssignOutcome.PirateUnavailable:
                    return Problem(detail: "Pirate is not available.", statusCode: 409);
                case AssignOutcome.PirateAlreadyAssigned:
                    return Problem(detail: "Pirate already has an active assignment.", statusCode: 409);
                case AssignOutcome.CrewCapacityReached:
                    return Problem(detail: "Expedition capacity reached.", statusCode: 409);
                default:
                    return Problem(detail: "Simulated failure. No data was modified.", statusCode: 500);
            }
        }


    }    
}
