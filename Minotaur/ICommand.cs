using System;
using System.Collections.Generic;

namespace MinotaurLabyrinth
{
	// An interface to represent one of many commands in the game. Each new command should
	// implement this interface.
	public interface ICommand
	{
		void Execute(LabyrinthGame game);
	}

	public interface ISecretCommand : ICommand { }

	public class DebugMapCommand : ISecretCommand
    {
		public void Execute(LabyrinthGame game)
        {
			if (game.Player.DebugMode) { game.Player.DebugMode = false; }
			else { game.Player.DebugMode = true; }
		}
    }

	// Represents a movement command, along with a specific direction to move.
	public class MoveCommand : ICommand
	{
		// The direction to move.
		public Direction Direction { get; }

		// Creates a new movement command with a specific direction to move.
		public MoveCommand(Direction direction)
		{
			Direction = direction;
		}

		// Causes the player's position to be updated with a new position, shifted in the intended direction,
		// but only if the destination stays on the map. Otherwise, nothing happens.
		public void Execute(LabyrinthGame game)
		{
			Location currentLocation = game.Player.Location;
			Location newLocation = Direction switch
			{
				Direction.North => new Location(currentLocation.Floor, currentLocation.Row - 1, currentLocation.Column),
				Direction.South => new Location(currentLocation.Floor, currentLocation.Row + 1, currentLocation.Column),
				Direction.West => new Location(currentLocation.Floor, currentLocation.Row, currentLocation.Column - 1),
				Direction.East => new Location(currentLocation.Floor, currentLocation.Row, currentLocation.Column + 1)
			};

			if (game.Map.IsOnMap(newLocation, game.Map))
				game.Player.Location = newLocation;
			else
				ConsoleHelper.WriteLine("There is a wall there.", ConsoleColor.Red);
		}
	}

	// A command that represents a request to pick up the sword.
	public class GetSwordCommand : ICommand
	{
		// Retrieves the sword if the player is in the room with the sword. Otherwise, nothing happens.
		public void Execute(LabyrinthGame game)
		{
			if (game.Map.GetRoomTypeAtLocation(game.Player.Location, game.Map) == RoomType.Sword) game.PlayerHasSword = true;
			else ConsoleHelper.WriteLine("The sword is not in this room. There was no effect.", ConsoleColor.Red);
		}
	}
	public class StairsCommand: ICommand
    {
		public void Execute(LabyrinthGame game)
		{
			RoomType destination = RoomType.StairsUp;
			int direction = 1;
			string text = "down";

			if (game.CurrentRoom.Type == RoomType.StairsUp)
			{
				destination = RoomType.StairsDown;
				direction = -1;
				text = "up";
			}

			for (int x = 0; x < game.Map.Rows; x++)
			{
				for (int y = 0; y < game.Map.Columns; y++)
				{
					Location stairspoint = new(game.Player.Location.Floor + direction, x, y);
					if (game.Map.GetRoomTypeAtLocation(stairspoint, game.Map) == destination)
					{
						game.Player.Location = stairspoint;
						ConsoleHelper.WriteLine($"You traveled {text} the stairs", ConsoleColor.Cyan);
						return;
					}
				}
			}
		}
    }

	public class CommandList
	{
		private readonly Dictionary<ICommand, List<string>> _commands = new Dictionary<ICommand, List<string>>();
		public CommandList()
		{
			_commands.Add(new MoveCommand(Direction.North), new List<string>() { "n", "north" });
			_commands.Add(new MoveCommand(Direction.South), new List<string>() { "s", "south" });
			_commands.Add(new MoveCommand(Direction.East), new List<string>() { "e", "east" });
			_commands.Add(new MoveCommand(Direction.West), new List<string>() { "w", "west" });
			_commands.Add(new DebugMapCommand(), new List<string>() { "debug" });
		}

		public void AddCommand(ICommand command, List<string> inputs)
        {
			foreach (var kvp in _commands)
            {
				if (kvp.Key.GetType() == command.GetType())
                {
					return;
                }
            }
			_commands.Add(command, inputs);
        }
		
		public void RemoveCommand(ICommand command)
        {
			foreach (var kvp in _commands)
			{
				if (kvp.Key.GetType() == command.GetType())
				{
					_commands.Remove(kvp.Key);
				}
			}
        }

		public void ResetCommand()
		{
			foreach (var kvp in _commands) if (kvp.Key is not MoveCommand && kvp.Key is not DebugMapCommand) _commands.Remove(kvp.Key);
		}

		public ICommand? GetCommand(string input)
		{
			input = input.ToLower();
			foreach (var kvp in _commands)
			{
				foreach (string command in kvp.Value)
				{
					if (input == command.ToLower()) return kvp.Key;
				}
			}
			return null;
		}
		public override string ToString()
		{
			string ret = "";
//			int commandNo = 1;

			foreach (var kvp in _commands)
			{
				if (kvp.Key is not DebugMapCommand) 
				{
//				ret += $"{commandNo++}.";
					foreach (string command in kvp.Value)
					{
						ret += $"({command}) ";
					}
					ret += '\n';
				}
			}
			return ret;
		}
	}
}
