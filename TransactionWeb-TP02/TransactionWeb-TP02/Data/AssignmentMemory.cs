using TransactionWeb_TP02.Models;

namespace TransactionWeb_TP02.Data
{
    public class AssignmentMemory
    {
        private List<Assignment> _assignments = new();

        public List<Assignment> GetAssignments()
        {
            return _assignments;
        }
        public void Reset()
        {
            _assignments = new List<Assignment>();
        }
    }
}
