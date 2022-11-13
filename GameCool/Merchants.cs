using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCool
{
	class Merchant : IInteractable
	{
		protected string Name;
		double goldAmount;
		protected Inventory _inventory = new();
		/// <summary>
		/// giving amount of gold means money when we call a merchent then they will have some amount of money
		/// </summary>
		/// <param name="gold">amount of gold a currency to trade</param>
		public Merchant(double gold ){goldAmount = gold;}
		/// <summary>
		/// Adding item to merchent
		/// </summary>
		/// <param name="item">item that we have to add</param>
		public void Add(InventoryItem item){_inventory.Add(item);}
		public virtual string GetName(){return Name;}
		/// <summary>
		/// Ask name from player and store it
		/// </summary>
		/// <param name="player">current player playing game</param>
		/// <returns>name of player</returns>
		public void SetPlayerName(Player player)
        {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Please enter your name below ");
			Console.ResetColor();
            try
            {
				string input = Console.ReadLine();
				if(input != null)
                {
					player.EnterName(input);
                }
				else { throw new ItemNullException("Your name can't be empty"); }
			}
			catch (IOException)
			{
				Console.Clear();
				Console.WriteLine("You ran into a string IOException for wrong input ");
				SetPlayerName(player);
			}
			catch (OutOfMemoryException)
			{
				Console.Clear();
				Console.WriteLine("You ran into a string OutOfMemoryException for wrong input ");
				SetPlayerName(player);
			}
			catch (ArgumentException)
			{
				Console.Clear();
				Console.WriteLine("You ran into a string ArgumentException for wrong input ");
				SetPlayerName(player);
			}
        }
		/// <summary>
		/// Will check the inventory of merchant
		/// </summary>
		public void AvailableInventory()
        {
			Console.WriteLine(_inventory.ToString());
		}
		/// <summary>
		/// in this function player can buy, sell and use items
		/// in buying an item it will ask to player which one he wants to buy and then add to player's inventory by charging the amount of item with some logical conditions
		/// in selling an item it will ask to player which one he wants to sell and then add to merchant's inventory by charging the amount of item with some logical conditions
		/// Using an item will show the effect and increase/decrease the health if that item have the healing power
		/// </summary>
		/// <param name="player">instance of player class that will transcation with </param>
		/// <exception cref="ItemNullException">an execption to check weather the code is null then show error message</exception>
		public virtual void Interact(Player player)
		{
			string playerName = player.PlayerName();
			try
			{
					Console.WriteLine($"Welcome {playerName} to ");
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(GetName());
					Console.ResetColor();
					Console.WriteLine();
					PlayerBalance(player);
					Console.WriteLine("Below are the available items I can sell now.");
					AvailableInventory();
					Console.WriteLine("Would you like to buy anything from above yes--> for buying menu/ no--> for selling menu?");
					string userChoice = Console.ReadLine();
					if (userChoice != null)
					{
						if (UserYesOrNoChoice(player, userChoice) == 1)
						{
							string userBuySelection = Console.ReadLine();
							if (userBuySelection != null)
							{
								BuySwitch(player, userBuySelection);
								Console.WriteLine($"{playerName} your updated balance is {player.goldAmount}Golds");
							}
							else { throw new ItemNullException($"{playerName} your selection can't be empty"); }
						}
						else if (UserYesOrNoChoice(player, userChoice) == 0)
						{
							Console.WriteLine($"{player.PlayerName()} then do you want to sell -->yes or -->no?");
							string userSellYesNo = Console.ReadLine();
							if (userSellYesNo != null)
							{
								if (UserSellYesNo(userSellYesNo, player))
								{
									string userSellSelection = Console.ReadLine();
								    if(userSellSelection != null)
									{
										SellSwitch(player, userSellSelection);
										Console.WriteLine($"{player.PlayerName()} your updated balance is {player.goldAmount}Golds");
									}
									else { throw new ItemNullException($"{playerName} your selection can't be empty"); }									
								}
								else
								{
									Console.WriteLine("");
									Console.WriteLine($"{playerName} according to your selection you don't want to sell stuff");
								}
                        }
							else { throw new ItemNullException($"{playerName} your selection can't be empty"); }
						}//
						Console.WriteLine($"{playerName} would you like to use any item you have?");
						string useAnyItemChoice = Console.ReadLine();
						if (useAnyItemChoice != null)
						{
							if (UseAnyItemChoice(player, useAnyItemChoice) == 1)
							{
								Console.WriteLine($"Below are the available items {playerName} you can use now.");
								player.PrintInventory();
								Console.WriteLine($"{playerName} enter item which you want to use now.");
								string userUseSelection = Console.ReadLine();
								if (userUseSelection != null)
								{
									ItemUseSwitch(player, userUseSelection);
									Console.WriteLine();
									Console.WriteLine($"{playerName} have a good day!");
								}
								else { throw new ItemNullException($"{playerName} your selection can't be empty"); }
							}
						}
						else { throw new ItemNullException($"{playerName} your input can't be empty"); }
					}
					else { throw new ItemNullException($"{playerName} your selection can't be empty"); }
					
            }
            catch (IOException)
            {
				Console.Clear();
				Console.WriteLine("You ran into a string IOException for wrong input ");
				Interact(player);
            }
			catch (OutOfMemoryException)
			{
				Console.Clear();
				Console.WriteLine("You ran into a string OutOfMemoryException for wrong input ");
				Interact(player);
			}
            catch (ArgumentException)
            {
				Console.Clear();
				Console.WriteLine("You ran into a string ArgumentException for wrong input ");
				Interact(player);
			}
		}
		/// <summary>
		/// will return some text on user's selection
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="userChoice">input string we will have from user</param>
		/// <returns>int variable we will make condition on that int return</returns>
		private int UserYesOrNoChoice(Player player,string userChoice)
		    {
				switch (userChoice.ToLower())
				{
					case "yes":
						Console.WriteLine($"{player.PlayerName()} which one you want to buy?");
						return 1;
					case "no":
						return 0;
					default:
						Console.WriteLine("Your input is not a valid selection.");
						return -1;
				}

			}
		/// <summary>
		/// Another choice method that show the player's inventory as well
		/// </summary>
		/// <param name="userSellSelection">input string we will have from user</param>
		/// <param name="player">current player instance who is playing</param>
		private bool UserSellYesNo(string userSellSelection, Player player)
		{
			if (userSellSelection.ToLower().Equals("yes"))
			{
				Console.WriteLine($"Below are the available items  you can sell to us.");
				player.PrintInventory();
				Console.WriteLine($"{player.PlayerName()} enter item which you want to sell now. enter item name below");
				return true;
			}
            else
            {
				Console.WriteLine();
            }
			return false;
		}
		/// <summary>
		/// Another choice method that show some text to console
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="userChoice">input string we will have from user</param>
		/// <returns>int variable we will make condition on that int return</returns>
		private int UseAnyItemChoice(Player player, string userChoice)
		{
			switch (userChoice.ToLower())
			{
				case "yes":
					Console.WriteLine($"{player.PlayerName()} which one you want to Use?");
					return 1;
				case "no":
					Console.WriteLine($"{player.PlayerName()} Thanks game finished for now.");
					return 0;
				default:
					Console.WriteLine("Your input is not a valid selection.");
					return -1;
			}
		}
		/// <summary>
		/// here if merchnat will have the given item then we will make him sell to player
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="item"> an InventoryItem class type that will check incoming item </param>
		private void InsideBuySwitch(Player player, InventoryItem item)
        {
			if (_inventory.ContainsItem(item))
			{
				Buy(player,item);
				Console.WriteLine($"{player.PlayerName()} Thanks for buying {item.GetName()}");
			}
            else
            {
				Console.WriteLine($"{player.PlayerName()} we don't have your chosen item in our inventory");
			}
		}
		/// <summary>
		/// checking that if player contains the given item then make a sell transcation
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="item"> an InventoryItem class type that will check incoming item </param>
		private void InsideSellSwitch(Player player, InventoryItem item)
		{
			if (player.ContainItem(item))
			{
				Sell(player,item);
				Console.WriteLine($"{player.PlayerName()} Thanks for selling {item.GetName()}");
			}
			else
			{
				Console.WriteLine($"{player.PlayerName()} you don't have your chosen item in your inventory");
			}
		}
		/// <summary>
		/// Show player's balance to console output
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		private void PlayerBalance(Player player)
        {
			Console.WriteLine($"{player.PlayerName()} your current balance is {player.goldAmount} Golds");
        }
		/// <summary>
		/// Will help to choose player which item he want to buy
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="userInput">input string we will have from user</param>
		private void BuySwitch(Player player,string userInput)
        {
			InventoryItem item;
			switch (userInput.ToLower())
			{
				case "shield":
					item = new Shield();
					InsideBuySwitch(player, item);
					break;
				case "crossbow":
					item = new CrossBow();
					InsideBuySwitch(player, item);
					break;
				case "sword":
					item = new Sword();
					InsideBuySwitch(player, item);
					break;
				case "dragonsuit":
					item = new DragonSuit();
					InsideBuySwitch(player, item);
					break;
				case "map":
					item = new Map();
					InsideBuySwitch(player, item);
					break;
				case "healingpotion":
					item = new HealingPotion();
					InsideBuySwitch(player, item);
					break;
				case "revengepotion":
					item = new RevengePotion();
					InsideBuySwitch(player, item);
					break;
				case "wishpotion":
					item = new WishPotion();
					InsideBuySwitch(player, item);
					break;
				default:
					Console.WriteLine("Your selection is not correct.");
					break;
			}
		}
		/// <summary>
		/// Will help to choose player which item he want to sell 
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="userInput">input string we will have from user</param>
		private void SellSwitch(Player player, string userInput)
		{
			InventoryItem item;
			switch (userInput.ToLower())
			{
				case "shield":
					item = new Shield();
					InsideSellSwitch(player, item);
					break;
				case "crossbow":
					item = new CrossBow();
					InsideSellSwitch(player, item);
					break;
				case "sword":
					item = new Sword();
					InsideSellSwitch(player, item);
					break;
				case "dragonsuit":
					item = new DragonSuit();
					InsideSellSwitch(player, item);
					break;
				case "map":
					item = new Map();
					InsideSellSwitch(player, item);
					break;
				case "healingpotion":
					item = new HealingPotion();
					InsideSellSwitch(player, item);
					break;
				case "revengepotion":
					item = new RevengePotion();
					InsideSellSwitch(player, item);
					break;
				case "wishpotion":
					item = new WishPotion();
					InsideSellSwitch(player, item);
					break;
				default:
					Console.WriteLine("Your selection is not correct.");
					break;
			}
		}
		/// <summary>
		/// A method that will show what will happen when we use an item like it will print item's decription 
		/// and how much health effected 
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="item"> an InventoryItem class type that will check incoming item </param>
		private void UseItem(Player player,InventoryItem item)
        {
			if (player.ContainItem(item))
			{
				Console.WriteLine();
				Console.WriteLine($"You used the item and it does following");
				Console.WriteLine(item.GetUse());
				Console.WriteLine($"Your health effected by {item.GetAction()} health points");
				player.PutHealth(item.GetAction());
			}
			else
			{
				Console.WriteLine($"{player.PlayerName()} you don't have your chosen item in your inventory to use");
			}
        }
		/// <summary>
		/// Will help to choose player which item he want to Use
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="userInput">input string we will have from user</param>
		private void ItemUseSwitch(Player player, string userInput)
		{
			InventoryItem item;
			switch (userInput.ToLower())
			{
				case "shield":
					item = new Shield();
					UseItem(player, item);
					break;
				case "crossbow":
					item = new CrossBow();
					UseItem(player, item);
					break;
				case "sword":
					item = new Sword();
					UseItem(player, item);
					break;
				case "dragonsuit":
					item = new DragonSuit();
					UseItem(player, item);
					break;
				case "map":
					item = new Map();
					UseItem(player, item);
					break;
				case "healingpotion":
					item = new HealingPotion();
					UseItem(player, item);
					break;
				case "revengepotion":
					item = new RevengePotion();
					UseItem(player, item);
					break;
				case "wishpotion":
					item = new WishPotion();
					UseItem(player, item);
					break;
				default:
					Console.WriteLine("Your selection is invalid");
					break;
			}
		}
		/// <summary>
		/// checking if player's gold are enough to buy the item from merchant then her is doing a buy txn
		/// where player will be charged for the cost of the item and merchant will recieve the money
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="item"> an InventoryItem class type that will check incoming item </param>
		private void Buy(Player player,InventoryItem item)
		{
            if (player.goldAmount > item.GetAmount())
            {
				_inventory.Remove(item);
				player.AddItem(item);
				goldAmount += item.GetAmount();
				player.goldAmount -= item.GetAmount();
			}
            else
            {
				Console.WriteLine($"Sorry {player.PlayerName()}, because your available balance is {player.goldAmount} which is less then the cost of item to buy");
            }
		}
		/// <summary>
		/// checking if merchant's gold are enough to buy the item from player then player is doing a sell txn
		/// where merchant will be charged for the cost of the item and player will recieve the money
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		/// <param name="item"> an InventoryItem class type that will check incoming item </param>
		private void Sell(Player player,InventoryItem item)
		{
                if (goldAmount > item.GetAmount())
                {
					_inventory.Add(item);
					player.RemoveItem(item);
					goldAmount -= item.GetAmount();
					player.goldAmount += item.GetAmount();
				}
                else{Console.WriteLine($"Sorry I don't have money to buy {item.GetName()} for now come back later");}			
		}
	}

	class WeaponMerchant : Merchant
	{
		/// <summary>
		/// storing customised name of the merchant
		/// </summary>
		/// <returns>returning a customised name for merchnat</returns>
		public override string GetName()
        {
			   Name = "         W)      ww                                         \n"
					+ "         W)      ww                                         \n"
					+ "         W)  ww  ww e)EEEEE a)AAAA  p)PPPP   o)OOO  n)NNNN  \n"
					+ "         W)  ww  ww e)EEEE   a)AAA  p)   PP o)   OO n)   NN \n"
					+ "         W)  ww  ww e)      a)   A  p)   PP o)   OO n)   NN \n"
					+ "          W)ww www   e)EEEE  a)AAAA p)PPPP   o)OOO  n)   NN \n"
					+ "                                    p)                      \n"
					+ "                                    p)                      \n"
					+ "   M)mm mmm                          h)                        t)   \n"
					+ "  M)  mm  mm                         h)                      t)tTTT \n"
					+ "  M)  mm  mm e)EEEEE  r)RRR   c)CCCC h)HHHH  a)AAAA  n)NNNN    t)   \n"
					+ "  M)  mm  mm e)EEEE  r)   RR c)      h)   HH  a)AAA  n)   NN   t)   \n"
					+ "  M)      mm e)      r)      c)      h)   HH a)   A  n)   NN   t)   \n"
					+ "  M)      mm  e)EEEE r)       c)CCCC h)   HH  a)AAAA n)   NN   t)T  \n";
			return Name;
        }
		/// <summary>
		/// constructor calling base and assigning number of gold when called
		/// </summary>
		/// <param name="gold">amount of gold that will be assigned to merchant when we call it</param>
		public WeaponMerchant(double gold) : base(gold) { }
		/// <summary>
		/// overriding base method from merchant class so when this method called it will also have stuff according to current merchant
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		public override void Interact(Player player) {base.Interact(player);}
    }
	class ArmorMerchant :  Merchant
	{
		/// <summary>
		/// storing customised name of the merchant
		/// </summary>
		/// <returns>returning a customised name for merchnat</returns>
		public override string GetName()
		{
			Name = "               A)aa                                      \n"
				 + "              A)  aa                                     \n"
				 + "             A)    aa  r)RRR   m)MM MMM   o)OOO   r)RRR  \n"
				 + "             A)aaaaaa r)   RR m)  MM  MM o)   OO r)   RR \n"
				 + "             A)    aa r)      m)  MM  MM o)   OO r)      \n"
				 + "             A)    aa r)      m)      MM  o)OOO  r)      \n"
				 + "                                                         \n"
				 + "   M)mm mmm                          h)                        t)   \n"
				 + "  M)  mm  mm                         h)                      t)tTTT \n"
				 + "  M)  mm  mm e)EEEEE  r)RRR   c)CCCC h)HHHH  a)AAAA  n)NNNN    t)   \n"
				 + "  M)  mm  mm e)EEEE  r)   RR c)      h)   HH  a)AAA  n)   NN   t)   \n"
				 + "  M)      mm e)      r)      c)      h)   HH a)   A  n)   NN   t)   \n"
				 + "  M)      mm  e)EEEE r)       c)CCCC h)   HH  a)AAAA n)   NN   t)T  \n";
			return Name;
		}
		/// <summary>
		/// constructor calling base and assigning number of gold when called
		/// </summary>
		/// <param name="gold">amount of gold that will be assigned to merchant when we call it</param>
		public ArmorMerchant(double gold) : base(gold){}
		/// <summary>
		/// overriding base method from merchant class so when this method called it will also have stuff according to current merchant
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		public override void Interact(Player player){ base.Interact(player);}
	}
	class PotionMerchant : Merchant
	{
		/// <summary>
		/// storing customised name of the merchant
		/// </summary>
		/// <returns>returning a customised name for merchnat</returns>
		public override string GetName()
		{
			Name = "             P)ppppp            t)   ##                 \n"
				 + "             P)    pp         t)tTTT                    \n"
				 + "             P)ppppp   o)OOO    t)   i)  o)OOO  n)NNNN  \n"
				 + "             P)       o)   OO   t)   i) o)   OO n)   NN \n"
				 + "             P)       o)   OO   t)   i) o)   OO n)   NN \n"
				 + "             P)        o)OOO    t)T  i)  o)OOO  n)   NN \n"
				 + "                                                         \n"
				 + "   M)mm mmm                          h)                        t)   \n"
				 + "  M)  mm  mm                         h)                      t)tTTT \n"
				 + "  M)  mm  mm e)EEEEE  r)RRR   c)CCCC h)HHHH  a)AAAA  n)NNNN    t)   \n"
				 + "  M)  mm  mm e)EEEE  r)   RR c)      h)   HH  a)AAA  n)   NN   t)   \n"
				 + "  M)      mm e)      r)      c)      h)   HH a)   A  n)   NN   t)   \n"
				 + "  M)      mm  e)EEEE r)       c)CCCC h)   HH  a)AAAA n)   NN   t)T  \n";
			return Name;
		}
		/// <summary>
		/// constructor calling base and assigning number of gold when called
		/// </summary>
		/// <param name="gold">amount of gold that will be assigned to merchant when we call it</param>
		public PotionMerchant(double gold) : base(gold){}
		/// <summary>
		/// overriding base method from merchant class so when this method called it will also have stuff according to current merchant
		/// </summary>
		/// <param name="player">current player instance who is playing</param>
		public override void Interact(Player player) { base.Interact(player); }
	}
	class ItemNullException : Exception
	{
		public ItemNullException(string message) : base(string.Format($"{message}")) { }
	}
}
