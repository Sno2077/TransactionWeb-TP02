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

            List<Assignment> assignments = _assignmentMemory.GetAssignments();

            if (expedition.State != "planned")
                return AssignOutcome.ExpeditionNotOpen;
            if (!pirate.Available)
                return AssignOutcome.PirateUnavailable;
            if (assignments.Any(a => a.PirateId == pirateId && a.Active))
                return AssignOutcome.PirateAlreadyAssigned;

            int activeCount = assignments.Count(a => a.ExpeditionId == expeditionId && a.Active);
            if (activeCount >= expedition.Capacity)
                return AssignOutcome.CrewCapacityReached;

            assignment = new Assignment
            {
                Id = assignments.Count == 0 ? 1 : assignments.Max(a => a.Id) + 1,
                PirateId = pirateId,
                ExpeditionId = expeditionId,
                Active = true

            };

            assignments.Add(assignment);
            pirate.Available = false;
            _pirateMemory.SaveToFile();

            return AssignOutcome.Success;

        }
    }
}
