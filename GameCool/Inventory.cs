using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCool
{
	class Inventory
	{
		private List<InventoryItem> _itemsList= new();
		/// <summary>
		/// Adding item to a list in this class 
		/// </summary>
		/// <param name="item">item to be add in the list</param>
		public void Add(InventoryItem item) { _itemsList.Add(item); }
		/// <summary>
		/// Removing item from list of this class if that item exist then we will find its index and remove item by index
		/// </summary>
		/// <param name="item">incoming item to be removed</param>
		/// <returns>true if item removed otherwise will return false</returns>
		public bool Remove(InventoryItem item)
		{
			int index = -1;
			for(int i = 0; i < _itemsList.Count; i++)
            {
                if (_itemsList[i].GetName == item.GetName) { index= i; } //stored index in which list contain same item
            }
			if (index > -1)
            {
				_itemsList.RemoveAt(index);
				return true;
			}
			return false;
		}
		/// <summary>
		/// method checks that if list of this class conatins a map
		/// </summary>
		/// <returns>true if list of this class have Map otherwise will return false</returns>
		public bool HasMap() { return ContainsItem(new Map()); }
		/// <summary>
		/// method checks that if list of this class conatins a sword
		/// </summary>
		/// <returns>true if list of this class have sword otherwise will return false</returns>
		public bool HasSword() { return ContainsItem(new Sword()); }
		/// <summary>
		/// This method check if the list of Inventory class have the same item to item given in parameter
		/// </summary>
		/// <param name="item">incoming item to check</param>
		/// <returns>true if given item is in list otherwise will return false</returns>
		public bool ContainsItem(InventoryItem item)
        {
			foreach(var listItem in _itemsList) { if (item.GetName() == listItem.GetName()) { return true; } }
			return false;
        }
		public string PrintForPlayer()
        {
			string ret = "";
			foreach (var item in _itemsList)
			{
				ret = ret + item.GetName() + "\n";
			}
			return ret;
		}
		public override string ToString()
		{
			string ret = "";
			foreach (var item in _itemsList)
			{
				ret = ret + item.GetName() + "\n"
					+ $"\t=>It will charge you {item.GetAmount()} \n"
					+ $"\t=> Action: {item.GetUse()}\n";
			}
			return ret;
		}
	}

	class InventoryItem
	{
		protected ItemNames Name;
		protected double Amount;
		protected int Action;
		protected string Use;
		public InventoryItem(ItemNames name, double amount, int action, string use){ Name = name;	Amount = amount; Action = action;	Use = use;  }
		public ItemNames GetName() { return Name; }
		public double GetAmount() {	return Amount; }
		public int GetAction() { return Action;	}
		public string GetUse(){	return Use;	}


	}
	class Weapon : InventoryItem
	{
        public Weapon(ItemNames name, double amount, int action, string use) : base(name, amount, action, use) { }
	}
	class Potion : InventoryItem
	{
		public Potion(ItemNames name, double amount, int action, string use) : base(name, amount, action, use) { }
	}
	class Shield : InventoryItem
	{
		public Shield() : base(ItemNames.Shield,99.99,0,"To protect the player") { }
	}
	class CrossBow : Weapon
	{
		public CrossBow() : base(ItemNames.CrossBow, 149.99, 0, "Kill enemies in a single shot") { }
	}
	class Sword : Weapon
	{
		public Sword() : base(ItemNames.Sword, 49.99, 0, "To face one to one sword fight") { }
	}
	class DragonSuit : InventoryItem
	{
		public DragonSuit() : base(ItemNames.DragonSuit, 249.99, 0, "A ultra protective powerful armor suit that can reduce the player's damage by 2 times") { }
	}
	class Map : InventoryItem
	{
		public Map() : base(ItemNames.Map, 9.99, 0, "To show the correct way to victory") { }
	}
	class HealingPotion : Potion
	{
		public HealingPotion() : base(ItemNames.HealingPotion, 199.99, 100, "Increase Player health by 100") { }
	}
	class RevengePotion : Potion
	{
		public RevengePotion() : base(ItemNames.RevengePotion, 0.99, -30, "A mysterious potion that may effect health in positive and negetive way.") { }
	}
	class WishPotion : Potion
	{
		public WishPotion() : base(ItemNames.WishPotion, 299.99, 100, "A Wish potion to make a free item wish with free health increase by 100 .") { }
	}
	enum ItemNames
    {
		Shield, WishPotion, RevengePotion, HealingPotion, Map, DragonSuit, CrossBow, Sword
	}
}
