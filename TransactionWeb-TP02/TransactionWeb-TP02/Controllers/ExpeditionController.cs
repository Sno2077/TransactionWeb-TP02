using Microsoft.AspNetCore.Mvc;
using TransactionWeb_TP02.Data;
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
    }    
}
