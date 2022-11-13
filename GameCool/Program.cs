namespace GameCool
{
    class Program
    {
        public static void Main(string[] args)
        {
            Location location = new Location(21, 21);
            Player player = new Player(location);
            player.goldAmount = 300.0;
            player.AddItem(new Map());// giving map to player
			try
			{
				Console.WriteLine("From which merchent do you want to trade?");
				Console.WriteLine("1 - Weapon Merchant");
				Console.WriteLine("2 - Armor Merchant");
				Console.WriteLine("3 - Potion Merchant");
				int input = Convert.ToInt32(Console.ReadLine());
				Merchant merchentChoice = input switch
				{
					1 => new WeaponMerchant(500.0),
					2 => new ArmorMerchant(300.0),
					3 => new PotionMerchant(400.0),
				};
				if(input == 1)
                {
					merchentChoice.SetPlayerName(player);
					merchentChoice.Add(new CrossBow());//giving few items to weapon merchant
					merchentChoice.Add(new Sword());
					merchentChoice.Interact(player);
				}
				else if (input == 2)
				{
					merchentChoice.SetPlayerName(player);
					merchentChoice.Add(new Shield());//giving few items to armor merchant
					merchentChoice.Add(new DragonSuit());
					merchentChoice.Interact(player);
				}
				else if (input == 3)
				{
					merchentChoice.SetPlayerName(player);
					merchentChoice.Add(new HealingPotion());//giving few items to potion merchant
					merchentChoice.Add(new RevengePotion());
					merchentChoice.Add(new WishPotion());
					merchentChoice.Interact(player);
				}
			}
			catch (FormatException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("That is an invalid selection.");

			}
			catch (System.Runtime.CompilerServices.SwitchExpressionException)
			{
				Console.WriteLine("You fall into an error");
			}
		}
    }
   
}

