using System;
using System.Collections.Generic;
public class PlayerInventory
{
    public Item Weapon;

    public Item Armor;

    public List<Item> Accessories = new();
    
    private bool ContainsItem(Item item)
    {
        if (item == Weapon || item == Armor)
            return true;
        return Accessories.Contains(item);
    }
    public Player.Stats Equip(Item item, Player.Stats stats)
    {
        switch (item.Type)
        {
            case Item.ItemType.Weapon:
                if (Weapon != null)
                {
                    stats -= Weapon.Unequip();
                }
                Weapon = item;
                stats += Weapon.Equip();
                break;
            
            case Item.ItemType.Armor:
                if (Armor != null)
                {
                    stats -= Armor.Unequip();
                }
                Armor = item;
                stats += Armor.Equip();
                break;
            
            case Item.ItemType.Accessory:
                if (Accessories.Count >= 2)
                {
                    stats -= Accessories[0].Unequip();
                    Accessories.RemoveAt(0);
                }
                Accessories.Add(item);
                stats += item.Equip();
                break;

        }

        return stats;
    }
    public Player.Stats Unequip(Item item, Player.Stats stats)
    {
        if (!ContainsItem(item))
            throw new ArgumentException("items not in inventory");
        switch (item.Type)
        {
            case Item.ItemType.Weapon:
                stats -= Weapon.Unequip();
                Weapon = null;
                break;
            
            case Item.ItemType.Armor:
                stats -= Armor.Unequip();
                Armor = null;
                break;
            
            case Item.ItemType.Accessory:
                stats -= item.Unequip();
                Accessories.Remove(item);
                break;
        }

        return stats;
    }

    public List<Item> GetItemList()
    {
            List<Item> items = new();
        if (Weapon != null)
            items.Add(Weapon);
        if (Armor != null)
            items.Add(Armor);
        items.AddRange(Accessories);
        return items;
    }
}
