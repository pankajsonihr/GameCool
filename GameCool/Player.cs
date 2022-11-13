namespace GameCool
{
    public class Player
    {
        private const int MaxHealth = 100;

        private int _health = MaxHealth;
        public Inventory _inventory; 
        public float goldAmount { get; private set; }

        public bool TradeGold(float amount)
        {
            if ((goldAmount + amount) >= 0)
            {
                goldAmount += amount;
                return true;
            }
            return false;
        }

        // Indicates whether the player currently has the catacomb map.
        public bool HasMap
        {
            get => _inventory.HasMap();
        }

        // Indicates whether the player currently has the sword.
        public bool HasSword
        {
            get => _inventory.HasSword();
        }

        /**************************************************************************************************
             * You do not need to alter anything below here but you are free to do
             * For example - while under the effects of a potion of invulnerability, the player cannot die
         *************************************************************************************************/

        // Indicates whether the player is alive or not.
        public bool IsAlive
        {
            get => _health > 0;
        }

        // Represents the distance the player can sense danger.
        // Diagonal adjacent squares have a range of 2 from the player.
        public int SenseRange { get; } = 1;

        // Creates a new player that starts at the given location, with inventory.
        public Player(Location start, Inventory inventory)
        {
            Location = start;
            _inventory = inventory;
            goldAmount = 25;
        }

        public Player(Location start)
        {
            Location = start;
            _inventory = new Inventory(10, 20, 30);
            goldAmount = 25;
        }

        // The player's current location.
        public Location Location { get; set; }

        // Explains why a player died.
        public string CauseOfDeath { get; private set; }

        public void Kill(string cause)
        {
            _health = 0;
            CauseOfDeath = cause;
        }
    }

    // Represents a location in the 2D game world, based on its row and column.
    public record Location(int Row, int Column);
}

