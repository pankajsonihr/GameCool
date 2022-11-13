using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPhaseOne
{
    /// <summary>
    /// Inventory container class that holds a characters items.
    /// Ability to add/remove items.
    /// </summary>
    public class Inventory
    {
        public int MaxCount { get; }
        public float MaxVolume { get; }
        public float MaxWeight { get; }

        private List<InventoryItem> _items;

        public int CurrentCount { get; private set; }
        public float CurrentVolume { get; private set; }
        public float CurrentWeight { get; private set; }
        public float TotalValue { get; private set; }
    

        public Inventory(int maxCount, float maxVolume, float maxWeight)
        {
            MaxCount = maxCount;
            MaxVolume = maxVolume;
            MaxWeight = maxWeight;
            _items = new List<InventoryItem> { };
        }

        /// <summary>
        /// Method to detect the map in the inventory.
        /// </summary>
        /// <returns> True - map in inventory | False - map NOT in inventory </returns>
        public bool HasMap() { return _items.OfType<Map>().Any(); }

        /// <summary>
        /// Method to detect the sword in the inventory.
        /// </summary>
        /// <returns> True - sword in inventory | False - sword NOT in inventory </returns>
        public bool HasSword() { return _items.OfType<Sword>().Any(); }


        /// <summary>
        /// Adds the specified item to the inventory if within the count/volume/weight limits
        /// </summary>
        /// <param name="item"> The item to add to the inventory </param>
        /// <returns> True/False if the item was within limits or not </returns>

        public InventoryItem GetItemAtIndex(int itemIndex) { return _items[itemIndex]; }
        public bool Add(InventoryItem item)
        {
            if (CurrentCount >= MaxCount) return false;
            if (CurrentVolume + item.Volume > MaxVolume) return false;
            if (CurrentWeight + item.Weight > MaxWeight) return false;

            _items.Add(item);
            CurrentCount++;
            CurrentVolume += item.Volume;
            CurrentWeight += item.Weight;
            TotalValue += item.Value;
            return true;
        }

        /// <summary>
        /// Removes the specified item from the inventory if found
        /// </summary>
        /// <param name="item"> The item to add to the inventory </param>
        /// <returns> True/False if the item was within limits or not </returns>
        public bool Remove(InventoryItem item)
        {
            for (int itemIndex = 0; itemIndex < _items.Count; itemIndex++)
            {
                if (item.ToString() == _items[itemIndex].ToString()) 
                { 
                    _items.RemoveAt(itemIndex);
                    CurrentCount--;
                    CurrentVolume -= item.Volume;
                    CurrentWeight -= item.Weight;
                    TotalValue -= item.Value;
                    return true;
                }
            }
            return false;
        }

        public InventoryItem RemoveAtIndex(int itemIndex)
        {
            InventoryItem item = _items[itemIndex];

            CurrentCount--;
            CurrentVolume -= _items[itemIndex].Volume;
            CurrentWeight -= _items[itemIndex].Weight;
            TotalValue -= _items[itemIndex].Value;

            _items.RemoveAt(itemIndex);
            return item;
        }
     

        public IEnumerator<InventoryItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override string ToString()
        {
            if (_items.Count == 0) return "{\n\tEmpty\n}";
            string ret = "{";
            foreach (InventoryItem item in _items)
            {
                ret += $"\n\t- {item.ToString()} - {item.Value} G";
            }
            ret += "\n}\n";
            return ret;
        }

        public (string items, int itemCount) GetIndexedInventory()
        {
            string ret = "\nItem Index\t-\tItem Name\t-\tValue";
            int itemIndex = 1;
            foreach (InventoryItem item in _items)
            {
                ret += $"\n\t{itemIndex}\t-\t{item}\t\t-\t{item.Value} Gold coins";
                itemIndex++;
            }
            return (items: ret, itemCount: itemIndex);
        }
    }

    static class InventoryTester
    {
        static public void AddEquipment(Inventory inventory)
        {
            bool addMoreItems = true;
            do
            {
                Console.WriteLine($"Inventory is currently at {inventory.CurrentCount}/{inventory.MaxCount} items, {inventory.CurrentWeight}/{inventory.MaxWeight} weight," +
                                  $" {inventory.CurrentVolume}/{inventory.MaxVolume} volume, and {inventory.TotalValue} total value.");
                Console.WriteLine(inventory.ToString());
                Console.WriteLine("What do you want to add?");
                Console.WriteLine("1 - Sword");
                Console.WriteLine("2 - Map");
                Console.WriteLine("3 - Shield");
                Console.WriteLine("4 - Healing Potion");
                Console.WriteLine("5 - Next - Remove Items");

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    InventoryItem newItem = choice switch
                    {
                        1 => new Sword(),
                        2 => new Map(),
                        3 => new Shield(),
                        4 => new HealingPotion()
                    };
                    if (!inventory.Add(newItem))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not fit this item into the inventory.");
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is an invalid selection.");

                }
                catch (System.Runtime.CompilerServices.SwitchExpressionException)
                {
                    Console.WriteLine("Venturing Forth!");
                    addMoreItems = false;
                }
                Console.ResetColor();
            } while (addMoreItems);
        }

        static public void RemoveEquipment(Inventory inventory)
        {
            bool remMoreItems = true;
            do
            {
                Console.WriteLine($"Inventory is currently at {inventory.CurrentCount}/{inventory.MaxCount} items, {inventory.CurrentWeight}/{inventory.MaxWeight} weight," +
                                  $" {inventory.CurrentVolume}/{inventory.MaxVolume} volume, and {inventory.TotalValue} total value."); 
                Console.WriteLine(inventory.ToString());
                Console.WriteLine("What do you want to Remove?");
                Console.WriteLine("1 - Sword");
                Console.WriteLine("2 - Map");
                Console.WriteLine("3 - Shield");
                Console.WriteLine("4 - Healing Potion");
                Console.WriteLine("5 - Gather your inventory and venture forth");

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    InventoryItem remItem = choice switch
                    {
                        1 => new Sword(),
                        2 => new Map(),
                        3 => new Shield(),
                        4 => new HealingPotion()
                    };
                    if (!inventory.Remove(remItem))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not find this item into the inventory.");
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That is an invalid selection.");

                }
                catch (System.Runtime.CompilerServices.SwitchExpressionException)
                {
                    Console.WriteLine("Venturing Forth!");
                    remMoreItems = false;
                }
                Console.ResetColor();
            } while (remMoreItems);
        }
    }
}
