using System.Runtime.InteropServices;
using TransactionWeb_TP02.Data;
using TransactionWeb_TP02.Models;

namespace TransactionWeb_TP02.Services
{
    public enum AssignOutcome
    {
        Success,
        PirateNotFound,
        ExpeditionNotFound,
        ExpeditionNotOpen,
        PirateUnavailable,
        PirateAlreadyAssigned,
        CrewCapacityReached
    }
    public class AssignmentService
    {
        private readonly PirateMemory _pirateMemory;
        private readonly ExpeditionMemory _expeditionMemory;
        private readonly AssignmentMemory _assignmentMemory;

        public AssignmentService(PirateMemory pirateMemory, ExpeditionMemory expeditionMemory, AssignmentMemory assignmentMemory)
        {
            _pirateMemory = pirateMemory;
            _expeditionMemory = expeditionMemory;
            _assignmentMemory = assignmentMemory;
        }

        public AssignOutcome Assign(int pirateId, int expeditionId, out Assignment? assignment)
        {
            assignment = null;

            Pirate? pirate = _pirateMemory.GetPirates().Find(p => p.Id == pirateId);
            if (pirate is null)
                return AssignOutcome.PirateNotFound;

            Expedition? expedition = _expeditionMemory.GetExpeditions().Find(e => e.Id == expeditionId);
            if (expedition is null)
                return AssignOutcome.ExpeditionNotFound;

        }
    }
}
