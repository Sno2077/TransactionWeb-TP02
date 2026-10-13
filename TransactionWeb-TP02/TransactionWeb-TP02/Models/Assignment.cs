namespace TransactionWeb_TP02.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int PirateId { get; set; }
        public int ExpeditionId { get; set; }
        public bool Active { get; set; } = true;
    }
}
