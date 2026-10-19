using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Models;

namespace TransactionWeb_TP02.Services
{
    public enum WorkflowResult
    {
        Success,
        NotFound,
        Forbidden
    }

    public class ExpeditionWorkflowService
    {
        private readonly ExpeditionMemory _expeditionMemory;

        public ExpeditionWorkflowService(ExpeditionMemory expeditionMemory)
        {
            _expeditionMemory = expeditionMemory;
        }

        public WorkflowResult Cancel(int expeditionId)
        {
            Expedition? expedition = _expeditionMemory.GetExpeditions().Find(e => e.Id == expeditionId);

            if (expedition is null)
                return WorkflowResult.NotFound;

            if (expedition.State != "planned" && expedition.State != "open")
                return WorkflowResult.Forbidden;

            expedition.State = "cancelled";
            return WorkflowResult.Success;
        }
    }
}