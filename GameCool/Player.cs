namespace GameCool
{
	class Player
	{
		private string _name { get; set; }
		private const int MaxHealth = 100;
		public double goldAmount { get; protected set; }
		private int _health = MaxHealth;
		private Inventory _inventory = new();

		private bool _hasMap = true;
		private bool _hasSword = false;
		public void PutHealth(int health)
        {
			_health +=health;
			Console.WriteLine($"{_name} your new health is {_health}");
        }
		public string PlayerName()
        {
			return _name;
        }
		public void EnterName(string name)
        {
			_name = name;
        }
		public void AddItem(InventoryItem items)
        {
			_inventory.Add(items);
        }
		/// <summary>
		/// Removing item from player Inventory's list
		/// </summary>
		/// <param name="item">incoming item to be removed</param>
		/// <returns>true if item removed otherwise will return false</returns>
		public bool RemoveItem(InventoryItem item)
        {
            if (_inventory.Remove(item))
            {
				_inventory.Remove(item);
				return true;
			}
			return false;
		}
		/// <summary>
		/// This method check if the player Inventory list has the given item or not
		/// </summary>
		/// <param name="item">item to check that it exist</param>
		/// <returns>true if given item is in list otherwise will return false</returns>
		public bool ContainItem(InventoryItem item)
        {
            if (_inventory.ContainsItem(item)) { return true;  }
			return false;
        }
		public void PrintInventory()
        {
			Console.WriteLine(_inventory.PrintForPlayer());
        }
		
		// Indicates whether the player currently has the catacomb map.
		public bool HasMap
		{
			get => _inventory.HasMap();
			//set => _hasMap = value; // This needs to be adjusted to determine if the map is present in the Player inventory   
		}

		// Indicates whether the player currently has the sword.
		public bool HasSword
		{
			get => _inventory.HasSword();
			//set => _hasSword = value; // This needs to be adjusted to determine if the sword is present in the Player inventory
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

		// Creates a new player that starts at the given location.
		public Player(Location start) => Location = start;

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

