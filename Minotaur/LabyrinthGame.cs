using System;
namespace MinotaurLabyrinth
{
	// The minotaur labyrinth game. Tracks the progression of a single round of gameplay.
	public class LabyrinthGame
	{
		// The map being used by the game.
		public Map Map { get; }

		// The player playing the game.
		public Player Player { get; }

		// The list of monsters in the game.
		public List<Monster> Monsters { get; }

		// Whether the player has the sword yet or not. (Defaults to `false`.)
		public bool PlayerHasSword
		{
			get => Player.HasSword;
			set => Player.HasSword = value;
		}

		// Whether the player has the map. (Defaults to `true`.)
		public bool PlayerHasMap
		{
			get => Player.HasMap;
			set => Player.HasMap = value;
		}

		// A list of senses that the player can detect. Add to this collection in the constructor.
		public readonly ISense[] _senses;

		// Contains all the commands that a player can access.
		private readonly CommandList _commandList = new ();
		public CommandList GetCommandList() => _commandList;

		// Initializes a new game round with a specific map and player.
		public LabyrinthGame(Map map, Player player, List<Monster> monsters)
		{
			Map = map;
			Player = player;
			Monsters = monsters;

			// Each of these senses will be used during the game. Add new senses here.
			_senses = new ISense[]
			{
				new PitSense(),
				new MinoSense(),
				new StealySense(),
				new GoblinSense()
			};
		}

		// Runs the game one turn at a time.
		public void Run()
		{
			Display.ScreenUpdate(this);
			// This is the "game loop." Each turn runs through this `while` loop once.
			while (!HasWon && Player.IsAlive)
			{
				ICommand command = GetCommand();
				Console.Clear();
				ConsoleHelper.WriteLine("Status: ", ConsoleColor.White);

				// Player quits the game
				if (command == null) Player.Kill("You abandoned your quest.");

				// Valid command to execute
				else
				{
					command.Execute(this);
					_commandList.ResetCommand();

					Location startinglocation;
					do
					{
						if (PlayerHasMap) CurrentRoom.Discovered = true;
						startinglocation = Player.Location;
						for (int i = Monsters.Count - 1; i > -1; i--) if (Monsters[i].Location == Player.Location) Monsters[i].Activate(this); // Activates any monster that shares a location with the player
						CurrentRoom.Activate(this); // Activates the room in the player' current location
					}
					while (startinglocation != Player.Location);

				}
				Display.ScreenUpdate(this);
			}
			if (HasWon)
			{
				ConsoleHelper.WriteLine("You have claimed the magic sword, and you have escaped with your life!", ConsoleColor.DarkGreen);
				ConsoleHelper.WriteLine("You win!", ConsoleColor.DarkGreen);
			}
			else
			{
				ConsoleHelper.WriteLine(Player.CauseOfDeath, ConsoleColor.Red);
				ConsoleHelper.WriteLine("You lost.", ConsoleColor.Red);
			}
		}
		// Gets an `ICommand` object that represents the player's desires.
		private ICommand GetCommand()
		{
			while (true) // Until we get a legitimate command, keep asking.
			{
				ConsoleHelper.Write("What do you want to do? ", ConsoleColor.White);
				Console.ForegroundColor = ConsoleColor.Cyan;
				string input = Console.ReadLine().ToLower();
				if (input == "quit") return null;
				var command = _commandList.GetCommand(input);
				if (command == null) ConsoleHelper.WriteLine($"I did not understand '{input}'.", ConsoleColor.Red); // If the input is not found in the command list, we have no clue what the command was. Try again.
				else return command;
			}
		}
		public void UpdateCommand(ICommand command, List<string> inputs)
        {
			_commandList.AddCommand(command, new List<string>() { inputs[0], inputs[1] });
		}

		public bool HasWon => CurrentRoom.Type == RoomType.Entrance && PlayerHasSword;

		// Looks up what room the player is currently in.
		public NormalRoom CurrentRoom => Map.GetRoomAtLocation(Player.Location);
	}
}

public static class ConsoleHelper
{
// Changes to the specified color and then displays the text on its own line.
	public static void WriteLine(string text, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.WriteLine(text);
	}

// Changes to the specified color and then displays the text without moving to the next line.
	public static void Write(string text, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.Write(text);
	}
	public static void Write(MinotaurLabyrinth.DisplayDetails details)
	{
		Console.ForegroundColor = details.Color;
		Console.Write(details.Text);
	}
}