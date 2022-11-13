using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPhaseOne
{
    public class PotionMerchant : Merchant
    {
        public PotionMerchant(Location location) : base(location)
        {
            _inventory = new Inventory(30, 30, 30);
            _inventory.Add(new HealingPotion());
            _inventory.Add(new HealingPotion());

            goldAmount = 250;

            Location = location;
        }

        public PotionMerchant(Location location, Inventory inventory) : base(location, inventory)
        {
            _inventory = inventory;

            goldAmount = 250;

            Location = location;
        }
    }

    public class ArmorMerchant : Merchant
    {
        public ArmorMerchant(Location location) : base(location)
        {
            _inventory = new Inventory(30, 30, 30);
            _inventory.Add(new Shield());
            _inventory.Add(new Shield());

            goldAmount = 250;

            Location = location;
        }

        public ArmorMerchant(Location location, Inventory inventory) : base(location, inventory)
        {
            _inventory = inventory;

            goldAmount = 250;

            Location = location;
        }
    }

    public class WeaponMerchant : Merchant
    {
        public WeaponMerchant(Location location) : base(location)
        {
            _inventory = new Inventory(30, 30, 30);
            _inventory.Add(new Sword());
            _inventory.Add(new Sword());

            goldAmount = 250;

            Location = location;
        }

        public WeaponMerchant(Location location, Inventory inventory) : base(location, inventory)
        {
            _inventory = inventory;

            goldAmount = 250;

            Location = location;
        }
    }

    public class Merchant : IInteractable
    {
        protected Inventory _inventory;
        public Location Location { get; set; }
        public float goldAmount { get; protected set; }

        public Merchant(Location location, Inventory inventory)
        {
            _inventory = inventory;
            goldAmount = 250;
            Location = location;
        }

        public Merchant(Location location)
        {
            _inventory = new Inventory(30, 30, 30);
            _inventory.Add(new Sword());
            _inventory.Add(new Shield());
            _inventory.Add(new Map());
            _inventory.Add(new HealingPotion());

            goldAmount = 250;

            Location = location;
        }

        public bool TradeGold(float amount)
        {
            if ((goldAmount + amount) >= 0)
            {
                goldAmount += amount;
                return true;
            }
            return false;
        }

        public void Interact(Player player)
        {
            bool interactMore = true;
            do
            {
                Console.WriteLine("\nYou approach the merchant. Before you can utter a word, he mutters \"Buying or selling? I've got all the odds and ends.\"");
                Console.WriteLine("\nWould do you want to do?");
                Console.WriteLine("1 - Buy");
                Console.WriteLine("2 - Sell");
                Console.WriteLine("3 - Leave\n");
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            SellMenu(player);
                            break;
                        case 2:
                            BuyMenu(player);
                            break;
                        case 3:
                            throw new System.Runtime.CompilerServices.SwitchExpressionException();
                        default:
                            throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is an invalid selection.");
                }
                catch (System.Runtime.CompilerServices.SwitchExpressionException)
                {
                    Console.WriteLine("\nSee you next time... if the dark lord doesn't first.");
                    interactMore = false;
                }
                Console.ResetColor();
            } while (interactMore);
        }

        void SellMenu(Player player)
        {
            bool sellMore = true;
            do
            {
                (string items, int itemCount) indexedInventory = _inventory.GetIndexedInventory();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nInventory is currently at {player._inventory.CurrentCount}/{player._inventory.MaxCount} items, {player._inventory.CurrentWeight}/{player._inventory.MaxWeight} weight," +
                                  $" {player._inventory.CurrentVolume}/{player._inventory.MaxVolume} volume, and {player._inventory.TotalValue} total value. You have {player.goldAmount} gold coins.");
                Console.ResetColor();
                Console.WriteLine("\n\"Here are my wares...\" Says the merchant.");
                Console.WriteLine("\nEnter the item index of the item you would like to purchase");
                Console.WriteLine(indexedInventory.items);
                Console.WriteLine($"\t{indexedInventory.itemCount}\t-\tExit\n");
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice >= 1 & choice <= indexedInventory.itemCount - 1)
                    {
                        SellItem(player, choice);
                    }
                    else if (choice == indexedInventory.itemCount)
                    {
                        throw new System.Runtime.CompilerServices.SwitchExpressionException();
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is an invalid selection.");
                }
                catch (System.Runtime.CompilerServices.SwitchExpressionException)
                {
                    Console.WriteLine("\n\"Hmm... Anything else?\" Says the merchant.");
                    sellMore = false;
                }
                Console.ResetColor();
            } while (sellMore);
        }

        bool SellItem(Player player, int itemIndex)
        {
            itemIndex = itemIndex - 1;
            float itemValue = _inventory.GetItemAtIndex(itemIndex).Value;
            InventoryItem item = _inventory.GetItemAtIndex(itemIndex);

            if (player.goldAmount >= itemValue)
            {
                item = _inventory.RemoveAtIndex(itemIndex);

                //Remove gold from players inventory
                player.TradeGold(itemValue * -1);
                //Add gold from this merchant
                TradeGold(itemValue);

                if (player._inventory.Add(item))
                {
                    Console.WriteLine($"\nYou have purchased a {item} for {itemValue} gold coins.\n");
                    return true;
                }
                else { return false; }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nYou have {player.goldAmount} gold coins, you need {itemValue} gold coins to purchase the {item}.\n");
            }            
            return false;
        }

        void BuyMenu(Player player)
        {
            bool buyMore = true;
            do
            {
                (string items, int itemCount) indexedInventory = player._inventory.GetIndexedInventory();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nYour inventory is currently at {player._inventory.CurrentCount}/{player._inventory.MaxCount} items, {player._inventory.CurrentWeight}/{player._inventory.MaxWeight} weight," +
                                  $" {player._inventory.CurrentVolume}/{player._inventory.MaxVolume} volume, and {player._inventory.TotalValue} total value. You have {player.goldAmount} gold coins.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\nThe merchants inventory is currently at {_inventory.CurrentCount}/{_inventory.MaxCount} items, {_inventory.CurrentWeight}/{_inventory.MaxWeight} weight," +
                                  $" {_inventory.CurrentVolume}/{_inventory.MaxVolume} volume, and {_inventory.TotalValue} total value. He has {goldAmount} gold coins.");
                Console.ResetColor();
                Console.WriteLine("\n\"Here are my wares...\" You say.");
                Console.WriteLine("\nEnter the item index of the item you would like to sell");
                Console.WriteLine(indexedInventory.items);
                Console.WriteLine($"\t{indexedInventory.itemCount}\t-\tExit\n");
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice >= 1 & choice <= indexedInventory.itemCount - 1)
                    {
                        BuyItem(player, choice);
                    }
                    else if (choice == indexedInventory.itemCount)
                    {
                        throw new System.Runtime.CompilerServices.SwitchExpressionException();
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is an invalid selection.");
                }
                catch (System.Runtime.CompilerServices.SwitchExpressionException)
                {
                    Console.WriteLine("\nHmm... Anything else?");
                    buyMore = false;
                }
                Console.ResetColor();
            } while (buyMore);
        }

        bool BuyItem(Player player, int itemIndex)
        {
            itemIndex = itemIndex - 1;
            InventoryItem item = _inventory.GetItemAtIndex(itemIndex);

            if (goldAmount >= item.Value)
            {
                item = player._inventory.RemoveAtIndex(itemIndex);

                //Remove gold from this inventory
                TradeGold(item.Value * -1);
                //Add gold to the player inventory
                player.TradeGold(item.Value);

                if (_inventory.Add(item))
                {
                    Console.WriteLine($"\nYou have Sold a {item} for {item.Value} gold coins.\n");
                    return true;
                }
                else { return false; }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThe merchant has {goldAmount} gold coins, he needs {item.Value} gold coins to purchase your {item}.\n");
            }
            return false;
        }        
    }
}
