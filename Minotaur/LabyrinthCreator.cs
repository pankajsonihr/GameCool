namespace MinotaurLabyrinth
{
	public static class LabyrinthCreator
	{
		static readonly (int floors, int rows, int cols) smallCoords = (2, 4, 4);
		static readonly (int floors, int rows, int cols) medCoords = (3, 6, 6);
		static readonly (int floors, int rows, int cols) largeCoords = (4, 8, 8);

		// Creates a small game using the smallCoords values.
		public static LabyrinthGame CreateSmallGame()
		{
			(Map map, Location start) = InitializeMap(Size.Small);
			return CreateGame(map, start);
		}

		// Creates a medium game using the medCoords values.
		public static LabyrinthGame CreateMediumGame()
		{
			(Map map, Location start) = InitializeMap(Size.Medium);
			return CreateGame(map, start);
		}

		// Creates a large game using the largeCoords values.
		public static LabyrinthGame CreateLargeGame()
		{
			(Map map, Location start) = InitializeMap(Size.Large);
			return CreateGame(map, start);
		}

		// Helper function that initializes the map size and all the map locations
		// Returns the initialized map and the entrance location (start) so we can
		// set the player starting location accordingly.
		private static (Map, Location) InitializeMap(Size mapSize)
		{
			Map map = mapSize switch
            {
                Size.Small => new Map(smallCoords.floors, smallCoords.rows, smallCoords.cols),
				Size.Medium => new Map(medCoords.floors, medCoords.rows, medCoords.cols),
				_ => new Map(largeCoords.floors, largeCoords.rows, largeCoords.cols),
				
			};
			Location start = RandomizeMap(map);
			return (map, start);
		}

		private static LabyrinthGame CreateGame(Map map, Location start)
		{
			Player player = InitializePlayer(start);
			List<Monster> monsters = InitializeMonsters(map);
			return new LabyrinthGame(map, player, monsters);
		}

		private static Player InitializePlayer(Location start) => new Player(start);

		// Generates monsters, one minotaur and an increasing number of others based on map size
		private static List<Monster> InitializeMonsters(Map map)
		{
			List<Monster> monsters = new();
			var rng = new RoomRng();
			for (int z = 0; z < map.Floors; z++)
			{
				monsters.Add(new Minotaur(rng.GetMonsterLocation(map, monsters, z)));
				monsters.Add(new Stealy(rng.GetMonsterLocation(map, monsters, z)));
				for (int i = 0; i < map.Columns / 3; i++) monsters.Add(new Goblin(rng.GetMonsterLocation(map, monsters, z)));
			}
			return monsters;
		}

		// Creates a map with randomly placed and non-overlapping features
		// The Entrance will be located in a random position along the edge of the map
		// The Sword will be placed randomly in the map but it will not be adjacent to the Entrance
		// Traps will be placed randomly, the number of them is based on half of the map' column size
		// Monsters will be placed randomly
		static private Location RandomizeMap(Map map)
		{
			var rng = new RoomRng();

			Location start = rng.SetStartLocation(map);
			map.SetRoomAtLocation(start, new EntranceRoom());

			for (int z = 0; z < map.Floors; z++)
			{
				map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new WarpRoom());
				for (int i = 0; i < map.Columns / 3; i++) map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new PitRoom());

				if (z == 0)		// First floor logic
				{
					map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new StairsDown());
				}
				else if (z == map.Floors - 1)		// Last floor logic
				{
					map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new SwordRoom());
					map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new StairsUp());
				}
                else        // Middle floor logic
				{
					map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new StairsUp());
					map.SetRoomAtLocation(rng.GetRoomLocation(map, false, z), new StairsDown());
				}
			}
			return start;
		}
		private enum Size { Small, Medium, Large };
	}
}