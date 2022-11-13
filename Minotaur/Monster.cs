namespace MinotaurLabyrinth
{
	/// <summary>
	/// Represents one of the several monster types in the game.
	/// </summary>
	public abstract class Monster
	{
		// The monster's current location.
		public Location Location { get; set; }

		// Whether the monster is alive or not.
		public bool IsAlive { get; set; } = true;

		// Creates a monster at the given location.
		protected Monster(Location start) => Location = start;

		// Called when generating the debug map
		// Returns a string which is the first letter of a monster type, and the associated color
		public abstract DisplayDetails DisplayMonster();

		// Called when the monster and the player are both in the same room. Gives
		// the monster a chance to do its thing.
		public abstract void Activate(LabyrinthGame game);
	}

	public class Minotaur : Monster		// Minotaur monster, knocks the player into another room and returns the sword if it's being carried, then runs off.
	{
		public Minotaur(Location start) : base(start) { }
		public override DisplayDetails DisplayMonster() => new("(M)", ConsoleColor.Magenta);
		public override void Activate(LabyrinthGame game)
		{
			var rng = new RoomRng();
			Location currentloc = game.Player.Location;		// Used to ensure player wakes up in a different room after being knocked out

			ConsoleHelper.WriteLine("You have encountered the minotaur! He charges at you and knocks you into another room.", ConsoleColor.Magenta);

			if (game.PlayerHasSword)
			{
				game.PlayerHasSword = false;
				ConsoleHelper.WriteLine("After recovering your senses, you notice you are no longer in possession of the magic sword!", ConsoleColor.Magenta);
			}

			Location = rng.GetMonsterLocation(game.Map, game.Monsters, this.Location.Floor);	// The minotaur runs to another random room in the dungeon
			// Player gets knocked out and wakes up in a random
			do
			{
				game.Player.Location = rng.GetRoomLocation(game.Map, false, this.Location.Floor);
			}
			while (game.Player.Location == this.Location || game.Player.Location == currentloc);
		}
	}

	public class Stealy : Monster		// Mr.Stealy! If the player encounters this monster, he'll steal their map and run away.
	{
		public Stealy(Location start) : base(start) { }
		public override DisplayDetails DisplayMonster() => new ("(S)", ConsoleColor.DarkYellow);
		public override void Activate(LabyrinthGame game)
		{
			var rng = new RoomRng();
			ConsoleHelper.WriteLine("You have encountered Mr.Stealy! He knocks you down and runs away!", ConsoleColor.Yellow);

			if (game.PlayerHasMap)
			{
				game.PlayerHasMap = false;
				game.Map.ClearMap();
				ConsoleHelper.WriteLine("After checking your pockets, you notice he stole your map!", ConsoleColor.Yellow);
			}
			Location = rng.GetMonsterLocation(game.Map, game.Monsters, this.Location.Floor);
		}
	}
	public class Goblin : Monster		// Goblin monster! If the player encounters this monster without the sword, they'll be killed. Otherwise, the player will kill the monster.
	{
		public Goblin(Location start) : base(start) { }
		public override DisplayDetails DisplayMonster() => new ("(G)", ConsoleColor.DarkRed);
		public override void Activate(LabyrinthGame game)
		{
			if (game.PlayerHasSword)
			{
				ConsoleHelper.WriteLine("You've encountered a goblin, fortunately the magic sword made quick work of him!", ConsoleColor.Cyan);
				game.Monsters.Remove(this);
			}
			else
			{
				game.Player.Kill("'A goblin! If only I had a sword!' you think, before realizing you've been dissemboweled... better luck next time!");
			}
		}
	}
}
