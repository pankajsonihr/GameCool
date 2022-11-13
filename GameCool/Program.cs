namespace GameCool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int packMaxItems = 10;
            float packMaxVolume = 20;
            float packMaxWeight = 30;
            //Creates the player inventory
            Inventory pack = new Inventory(packMaxItems, packMaxVolume, packMaxWeight);
            pack.Add(new Sword());
            pack.Add(new Map());

            //Creates the merchant inventory
            Inventory pack2 = new Inventory(packMaxItems, packMaxVolume, packMaxWeight);
            pack2.Add(new Shield());
            pack2.Add(new HealingPotion());

            //Test the add/remove features of the inventory + sword/map check
            //InventoryTester.AddEquipment(pack);
            //InventoryTester.RemoveEquipment(pack);
            //Console.WriteLine($"\t - Has Map: {pack.HasMap()}");
            //Console.WriteLine($"\t - Has Sword: {pack.HasSword()}");

            //Creates player and merchant, attaches the proper inventories.
            Player player = new Player(new Location(0, 0), pack);
            Merchant merch1 = new Merchant(new Location(0, 0));

            //Interact test
            merch1.Interact(player);

            PotionMerchant merch2 = new PotionMerchant(new Location(0, 0));
            ArmorMerchant merch3 = new ArmorMerchant(new Location(0, 0));
            WeaponMerchant merch4 = new WeaponMerchant(new Location(0, 0));
        }
    }
}