using System;
namespace MinotaurLabyrinth
{
    static public class Display
    {
		// Updates the display screen each run
		static public void ScreenUpdate(LabyrinthGame game)
		{
			ConsoleHelper.WriteLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
			ConsoleHelper.WriteLine($"Map Floor: {game.Player.Location.Floor}", ConsoleColor.White);
			DisplayMap(game);
			ConsoleHelper.WriteLine("\nCommands:", ConsoleColor.White);
			ConsoleHelper.Write($"{game.GetCommandList()}\n", ConsoleColor.White);
			ConsoleHelper.WriteLine("Senses:", ConsoleColor.White);
			DisplayStatus(game);
			ConsoleHelper.WriteLine("--------------------------------------------------------------------------------\n", ConsoleColor.Gray);
		}
		// Displays the status to the player, including what room they are in and asks each sense to display itself
		// if it is currently relevant.
		static public void DisplayStatus(LabyrinthGame game)
		{
			bool somethingSensed = CheckSenses(game);
			if (!somethingSensed) ConsoleHelper.WriteLine($"You sense nothing of interest nearby.", ConsoleColor.Gray);
			if (game.Player.HasSword) ConsoleHelper.WriteLine($"You are currently carrying the sword! Make haste for the exit!", ConsoleColor.DarkYellow);
		}
		// Asks each sense to display itself if relevant. Returns true if something is sensed and false otherwise.
		static public bool CheckSenses(LabyrinthGame game)
		{
			bool somethingSensed = false;
			foreach (ISense sense in game._senses)
			{
				if (sense.CanSense(game))
				{
					somethingSensed = true;
					sense.DisplaySense(game);
				}
			}
			return somethingSensed;
		}
		// This function displays the map each round, depending on if the playerhasmap property and the debug 
		static public void DisplayMap(LabyrinthGame game)
		{
			for (int i = 0; i < game.Map.Rows; ++i)
			{
				for (int j = 0; j < game.Map.Columns; ++j)
				{
					Location location = new(game.Player.Location.Floor, i, j);
					if (location == game.Player.Location) ConsoleHelper.Write("(X)", System.ConsoleColor.Yellow);
                    else
                    {
						var room = game.Map.GetRoomAtLocation(location);

						if (game.Player.DebugMode)
						{
							var monstertype = game.Map.MonsterCheck(game.Player.Location.Floor, i, j, game.Monsters);
							if (monstertype != null) ConsoleHelper.Write(monstertype.DisplayMonster());
							else ConsoleHelper.Write(room.DisplayRoom());
						}

						else if (game.Player.HasMap)
						{
							if (room.Type == RoomType.Entrance || room.Discovered == true) ConsoleHelper.Write(room.DisplayRoom());
							else ConsoleHelper.Write($"[ ]", System.ConsoleColor.Gray);
						}
						else ConsoleHelper.Write($"[ ]", System.ConsoleColor.Black);
					}
				}
				System.Console.WriteLine();
			}
		}
	}
	public record DisplayDetails(string Text, ConsoleColor Color);
}