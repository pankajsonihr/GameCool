using System;

// Random number generation class

namespace MinotaurLabyrinth
{
    class RoomRng
    {
        private readonly Random _rng = new ();

        // Generates an entrance along the walls of the map using a random number from 1-4 to represent the four sides
        public Location SetStartLocation(Map map)
        {
            int startRow;
            int startCol;
            if (_rng.Next(2) == 0)
            {
                startRow = _rng.Next(map.Rows);
                startCol = (_rng.NextDouble() < 0.5) ? 0 : map.Columns - 1;
            }
            else
            {
                startCol = _rng.Next(map.Columns);
                startRow = (_rng.NextDouble() < 0.5) ? 0 : map.Rows - 1;
            }
            Location startLocation = new (0, startRow, startCol);
            return startLocation;
        }

        // Returns a normal room; passing True to nearEntrance allows a room to be next to the Entrance room.
        public Location GetRoomLocation(Map map, bool nearEntrance, int floor)
        {
            Location loc;
            if (nearEntrance)
            {
                do loc = new Location(floor, _rng.Next(map.Columns), _rng.Next(map.Rows));
                while (!(map.GetRoomTypeAtLocation(loc, map) == RoomType.Normal));
                return loc;
            }
            else
            {
                do loc = new Location(floor, _rng.Next(map.Columns), _rng.Next(map.Rows));
                while ((map.HasNeighborWithType(loc, RoomType.Entrance, map)) || !(map.GetRoomTypeAtLocation(loc, map) == RoomType.Normal));
                return loc;
            }
        }
        // Returns a room that isn't next to the entrance, a special room, or occupied by another monster
        public Location GetMonsterLocation(Map map, List<Monster> monsters, int floor)
        { 
            Location loc;
            do loc = new Location(floor, _rng.Next(map.Columns), _rng.Next(map.Rows));
            while (map.HasNeighborWithType(loc, RoomType.Entrance, map) || !(map.GetRoomTypeAtLocation(loc, map) == RoomType.Normal) || (map.MonsterCheck(floor, loc.Row, loc.Column, monsters) != null));
            return loc;
        }
    }
}