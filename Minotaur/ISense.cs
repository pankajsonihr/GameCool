using System;

namespace MinotaurLabyrinth
{
	// Represents something that the player can sense as they wander the labyrinth.
	public interface ISense
	{
		// Returns whether the player should be able to sense the thing in question.
		bool CanSense(LabyrinthGame game);

		// Displays the sensed information. (Assumes `CanSense` was called first and returned `true`.)
		void DisplaySense(LabyrinthGame game);
	}
	public class PitSense : ISense
	{
		// Returns `true` if the player is near a trap pit
		public bool CanSense(LabyrinthGame game) => game.Map.HasNeighborWithType(game.Player.Location, RoomType.Pit, game.Map);

		// Displays the appropriate message if the player can sense a nearby trap pit
		public void DisplaySense(LabyrinthGame game) => ConsoleHelper.WriteLine("You can sense a trap nearby, be careful!", ConsoleColor.DarkRed);
	}

	// Allows the player to sense the minotaur if in a neighboring room
	public class MinoSense : ISense
	{
		// Returns `true` if the player is near the minotaur
		public bool CanSense(LabyrinthGame game) => game.Map.MonsterNear(game.Player.Location, game.Monsters, typeof(Minotaur));

		// Displays the appropriate message if the player can sense the minotaur nearby
		public void DisplaySense(LabyrinthGame game) => ConsoleHelper.WriteLine("You can sense the minotaur nearby, be careful!", ConsoleColor.DarkMagenta);
	}
	public class StealySense : ISense
	{
		// Returns `true` if the player is near Mr.Stealy
		public bool CanSense(LabyrinthGame game) => game.Map.MonsterNear(game.Player.Location, game.Monsters, typeof(Stealy));

		// Displays the appropriate message if the player can sense Mr.Stealy nearby
		public void DisplaySense(LabyrinthGame game) => ConsoleHelper.WriteLine("You smell a dirty thief nearby!", ConsoleColor.DarkYellow);

	}
	public class GoblinSense : ISense
	{
		// Returns `true` if the player is near a goblin
		public bool CanSense(LabyrinthGame game) => game.Map.MonsterNear(game.Player.Location, game.Monsters, typeof(Goblin));

		// Displays the appropriate message if the player can sense a goblin nearby
		public void DisplaySense(LabyrinthGame game) => ConsoleHelper.WriteLine("You sense a foul odor...", ConsoleColor.DarkRed);
	}
}
