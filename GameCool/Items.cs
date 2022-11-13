using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GameCool
{
    public class InventoryItem
    {
        public float Weight { get; }
        public float Volume { get; }
        public float Value { get; }

        public InventoryItem() { }

        public InventoryItem(float weight, float volume, float value)
        {
            Weight = weight;
            Volume = volume;
            Value = value;
        }

        public override string ToString()
        {
            string ret = base.ToString();
            return ret.Substring(ret.IndexOf('.') + 1);
        }
    }

    class Weapon : InventoryItem 
    {
        public float Damage { get; }
        public Weapon(float weight, float volume, float value, float damage) : base(weight, volume, value) 
        {
            Damage = damage;
        } 
    }

    class Potion : InventoryItem
    {
        public PotionEffect Effect { get; }
        public float EffectValue { get; }

        public Potion(PotionEffect effect, float effectValue, float weight = 1.2f, float volume = 2, float value = 12) : base (weight, volume, value)
        {
            Effect = effect;
            EffectValue = effectValue;
        }

        public override string ToString()
        {
            string ret = $"Potion of {Effect}";
            return ret;
        }
    }

    class HealingPotion : Potion
    {
        public HealingPotion(float effectValue = 6.66f, float value = 12) : base(PotionEffect.Healing, effectValue, 1.2f, 2, value) { }
    }

    class Sword : Weapon 
    { 
        //Default values for the sword, can be adjusted.
        public Sword(float weight = 5, float volume = 7, float value = 15, float damage = 8.5f) : base(weight, volume, value, damage) { } 
    }

    class Shield : Weapon 
    { 
        public Shield(float weight = 7, float volume = 10, float value = 12.5f, float damage = 7) : base(weight, volume, value, damage) { } 
    }

    class Map : InventoryItem { public Map() : base(1, 0.5f, 5.5f) { } }
    
    //class Rope : InventoryItem { public Rope() : base(1, 1.5f, 8) { } }

    enum PotionEffect { Healing, Slowness, Poison }
}
