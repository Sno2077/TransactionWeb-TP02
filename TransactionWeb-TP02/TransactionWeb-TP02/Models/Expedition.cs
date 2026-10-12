namespace TransactionWeb_TP02.Models
{
    public class Expedition
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public string State { get; set; } = "planned";
    }
}
