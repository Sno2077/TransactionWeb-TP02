namespace TransactionWeb_TP02.Models
{
    public class Pirate
    {
        private int _id;
        private string _name;
        private string _type;
        private int _level;
        private int _bounty;
        private bool _marine;
        private bool _available;


        private static List<string> ValidTypes = new()
        {
            "fighter","swordsman","navigator","doctor","ingeneer","cook","sniper"
        };

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 80)
                    throw new ArgumentException("Name must be between 2 and 80 characters long.");

                _name = value;
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                if (!ValidTypes.Contains(value))
                    throw new ArgumentException($"Available types are : {string.Join(",", ValidTypes)}");

                _type = value;
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException("Level must be between 1-100");

                _level = value;
            }
        }


        public int Bounty
        {
            get { return _bounty; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Bounty is a negative number...");

                _bounty = value;
            }
        }

        public bool Marine
        {
            get { return _marine; }
            set { _marine = value; }
        }

        public bool Available
        {
            get { return _available; }
            set { _available = value; }
        }
    }
}
