using System.Text.Json;
using TransactionWeb_TP02.Models;

namespace TransactionWeb_TP02.Data
{
    public class PirateMemory
    {
        private List<Pirate> _pirates = new();
        private readonly string _filePath = "pirates.json";

        public PirateMemory()
        {
            LoadFromFile();
        }

        public List<Pirate> GetPirates()
        {
            return _pirates;
        }

        public void SaveToFile()
        {
            string json = JsonSerializer.Serialize(_pirates, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _pirates = JsonSerializer.Deserialize<List<Pirate>>(json) ?? new List<Pirate>();
            }
            else
            {
                _pirates = new List<Pirate>
                {
                    new Pirate { Id = 1, Name = "Luffy", Level = 1, Type = "fighter", Bounty = 300000, Marine = false, Available = true },
                    new Pirate { Id = 2, Name = "Zoro", Level = 1, Type = "swordsman", Bounty = 250000, Marine = false, Available = true },
                    new Pirate { Id = 3, Name = "Garp", Level = 1, Type = "fighter", Bounty = 0, Marine = true, Available = true },
                    new Pirate { Id = 4, Name = "Bellamy", Level = 1, Type = "fighter", Bounty = 195000, Marine = false, Available = true }
                };
                SaveToFile();
            }
        }
    }
}