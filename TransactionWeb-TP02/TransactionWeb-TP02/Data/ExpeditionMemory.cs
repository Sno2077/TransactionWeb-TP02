using TransactionWeb_TP02.Models;

namespace TransactionWeb_TP02.Data
{
    public class ExpeditionMemory
    {
        private List<Expedition> _expeditions = new();

        public ExpeditionMemory()
        {
            Reset();
        }
                public List<Expedition> GetExpeditions()
        {
            return _expeditions;
        }

        public void Reset()
        {
            _expeditions = new List<Expedition>
            {
                new Expedition { Id = 1, Name = "Twin Cape", Capacity = 2, State = "open" },
                new Expedition { Id = 2, Name = "Water 7", Capacity = 4, State = "planned" },
                new Expedition { Id = 3, Name = "Alabasta", Capacity = 7, State = "planned" }
            };
        }
    }
}
