using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/SimpleItem")]
public class Item : ScriptableObject, ISellableItem
{
    public enum ItemType
    {
        Armor,
        Weapon,
        Accessory
    }
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private int _hpBoost;
    [SerializeField] private int _resistanceBoost;
    [SerializeField] private int _attackBoost;
    [SerializeField] private ItemType _type;
    public int Cost => _price;
    public string Name => _name;
    public virtual string Description => _description;
    public Sprite Icon => _icon;
    public ItemType Type => _type;

    protected virtual Player.Stats GetStats()
    {
        return new Player.Stats()
        {
            Damage = _attackBoost,
            Health = _hpBoost,
            Resist = _resistanceBoost
        };
    }

    public virtual Player.Stats Equip()
    {
        Debug.Log("Equipped");
        return GetStats();
    }

    public virtual Player.Stats Unequip()
    {
        Debug.Log("Unequipped"); 
        return GetStats();
    }

    public virtual void Use(Transform target)
    {
        Debug.Log("Used");
    }
    public void Buy(Player player)
    {
        player.EquipItem(Instantiate(this));
    }

    public override string ToString()
    {
        return Name;
    }
}
