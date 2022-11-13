namespace MinotaurLabyrinth
{
	// Represents the map and what each room is made out of.
	public class Map
	{
		// Stores which room type each room in the world is. The default is `Normal` because that is the first
		// member in the enumeration list.
		private NormalRoom[,,] Rooms { get; set; }

		// The total number of rows in this specific game world.
		public int Rows { get; }

		// The total number of columns in this specific game world.
		public int Columns { get; }
		// The total number of floors in this specific game world.
		public int Floors { get; }

		// Creates a new map with a specific size.
		public Map(int floors, int rows, int columns)
		{
			Rows = rows;
			Columns = columns;
			Floors = floors;
			Rooms = new NormalRoom[floors, rows, columns];
			for (int z = 0; z < floors; z++)
			{	
				for (int x = 0; x < rows; x++)
				{
					for (int y = 0; y < columns; y++)
					{
						Location loc = new(z, x, y);
						Rooms[z, x, y] = new NormalRoom();
					}
				}
			}
		}

		// Changes the type of room at a specific spot in the world to a new type.
		public void SetRoomAtLocation(Location location, NormalRoom room) => Rooms[location.Floor, location.Row, location.Column] = room;

		// Returns what type a room at a specific location is.
		public RoomType GetRoomTypeAtLocation(Location location, Map map) => IsOnMap(location, map) ? Rooms[location. Floor, location.Row, location.Column].Type : RoomType.OffTheMap;
		public NormalRoom GetRoomAtLocation(Location location) => Rooms[location.Floor, location.Row, location.Column];

		// Indicates whether a specific location is actually on the map or not.
		public bool IsOnMap(Location location, Map map) =>
			location.Floor >= 0 &&
			location.Floor < map.Floors &&
			location.Row >= 0 &&
			location.Row < map.Rows &&
			location.Column >= 0 &&
			location.Column < map.Columns;

		// Determines if a neighboring room is of the given type.
		public bool HasNeighborWithType(Location location, RoomType type, Map map)
		{
			if (GetRoomTypeAtLocation(new Location(location.Floor, location.Row - 1, location.Column), map) == type) return true;
			if (GetRoomTypeAtLocation(new Location(location.Floor, location.Row, location.Column - 1), map) == type) return true;
			if (GetRoomTypeAtLocation(new Location(location.Floor, location.Row, location.Column), map) == type) return true;
			if (GetRoomTypeAtLocation(new Location(location.Floor, location.Row, location.Column + 1), map) == type) return true;
			if (GetRoomTypeAtLocation(new Location(location.Floor, location.Row + 1, location.Column), map) == type) return true;
			return false;
		}

		// Used by the sense process to determine if a monster is in a neighboring room
		public bool MonsterNear(Location playerloc, List<Monster> monsters, Type monsterType)
        {
			for (int i = 0; i < monsters.Count; ++i)
			{
				if (monsters[i].GetType() == monsterType)
				{
					if (new Location(playerloc.Floor, playerloc.Row - 1, playerloc.Column) == monsters[i].Location) return true;
					if (new Location(playerloc.Floor, playerloc.Row, playerloc.Column - 1) == monsters[i].Location) return true;
					if (new Location(playerloc.Floor, playerloc.Row + 1, playerloc.Column) == monsters[i].Location) return true;
					if (new Location(playerloc.Floor, playerloc.Row, playerloc.Column + 1) == monsters[i].Location) return true;
				}
			}
			return false;
		}

		// Used by the debug tool to check monster locations against the map
		public Monster? MonsterCheck(int z, int x, int y, List<Monster> monsters)
		{
			Location location = new(z, x, y);
			foreach (Monster monster in monsters)
			{
				if (monster != null)
				{
					if (monster.Location == location)
					{ 
						return monster; 
					}
				}
			}
			return null;
		}
		public void ClearMap() { foreach (NormalRoom room in Rooms) room.Discovered = false; }
	}

// Represents a location in the 2D game world, based on its row and column.
public record Location(int Floor, int Row, int Column);

// Represents one of the four directions of movement.
public enum Direction { North, South, West, East }
}