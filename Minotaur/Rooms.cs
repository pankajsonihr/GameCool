using System;
using System.Collections.Generic;

namespace MinotaurLabyrinth
{
    public class NormalRoom
    {
        public RoomType Type { get; set; }
        public bool Discovered { get; set; } = false;
        public NormalRoom() { Type = RoomType.Normal; }
        public virtual DisplayDetails DisplayRoom() => new("[N]", ConsoleColor.Gray);
        public virtual void Activate(LabyrinthGame game) { }
    }
    public class EntranceRoom : NormalRoom
    {
        public EntranceRoom() { Type = RoomType.Entrance; }
        public override DisplayDetails DisplayRoom() => new("[E]", ConsoleColor.Blue);
        public override void Activate(LabyrinthGame game)
        {
            {
                if (!game.Player.HasMap)
                {
                    ConsoleHelper.WriteLine("Thankfully you've made it back to the entrance! The nearby village provides you with another map.", ConsoleColor.Blue);
                    game.Player.HasMap = true;
                }
                else
                {
                    ConsoleHelper.WriteLine("You see light in this room coming from outside the labyrinth. This is the entrance.", ConsoleColor.Yellow);
                }
            }
        }
    }
    public class SwordRoom : NormalRoom
    {
        public SwordRoom() { Type = RoomType.Sword; }
        public override DisplayDetails DisplayRoom() => new("[S]", ConsoleColor.Green);
        public override void Activate(LabyrinthGame game)
        {
            {
                // Displays the appropriate message depending on whether the sword is picked up or not.
                if (game.Player.HasSword) ConsoleHelper.WriteLine("This is the sword room but you've already picked up the sword!", ConsoleColor.DarkCyan);
                else
                {
                    ConsoleHelper.WriteLine("You found the magic sword!", ConsoleColor.DarkCyan);
                    game.UpdateCommand(new GetSwordCommand(), new List<string> { "g", "grab" });
                }
            }
        }
    }
    public class PitRoom : NormalRoom
    {
        public PitRoom() { Type = RoomType.Pit; }
        public override DisplayDetails DisplayRoom() => new("[P]", ConsoleColor.Red);
        public override void Activate(LabyrinthGame game)
        {
            game.Player.Kill("You fell into a trap and died!");
        }
    }
    public class WarpRoom : NormalRoom
    {
        public WarpRoom() { Type = RoomType.Warp; }
        public override DisplayDetails DisplayRoom() => new ("[W]", ConsoleColor.Cyan);
        public override void Activate(LabyrinthGame game)
        {
            ConsoleHelper.WriteLine("You've entered a portal room and warped back to the entrance!", ConsoleColor.Cyan);
            for (int x = 0; x < game.Map.Rows; ++x)
            {
                for (int y = 0; y < game.Map.Columns; ++y)
                {
                    Location startpoint = new (0, x, y);
                    if (game.Map.GetRoomTypeAtLocation(startpoint, game.Map) == RoomType.Entrance)
                    {
                        game.Player.Location = startpoint;
                    }
                }
            }
        }
    }
    public class StairsDown : NormalRoom
    {
        public StairsDown() { Type = RoomType.StairsDown; }
        public override DisplayDetails DisplayRoom() => new ("[D]", ConsoleColor.DarkCyan);
        public override void Activate(LabyrinthGame game)
        {
            ConsoleHelper.WriteLine("This room has stairs leading down", ConsoleColor.DarkCyan);
            game.UpdateCommand(new StairsCommand(), new List<string> { "d", "down"});
        }
    }
    public class StairsUp : NormalRoom
    {
        public StairsUp() { Type = RoomType.StairsUp; }
        public override DisplayDetails DisplayRoom() => new ("[U]", ConsoleColor.DarkCyan);
        public override void Activate(LabyrinthGame game)
        {
            ConsoleHelper.WriteLine("This room has stairs leading up", ConsoleColor.DarkCyan);
            game.UpdateCommand(new StairsCommand(), new List<string> { "u", "up" });
        }
    }

    // Represents one of the different types of rooms in the game.
    public enum RoomType { Normal, Entrance, Sword, Pit, Warp, StairsDown, StairsUp, OffTheMap }
}
