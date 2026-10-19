using System.ComponentModel.DataAnnotations;

namespace TransactionWeb_TP02.Models
{
    public class AssignmentDTO
    {
        [Range(1, int.MaxValue)]
        public int PirateId { get; set; }
    }
}