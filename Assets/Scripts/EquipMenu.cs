using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenu : MonoBehaviour
{
    [SerializeField] private Button _unequipButton;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private InventoryCell _weaponCell;
    [SerializeField] private InventoryCell _armorCell;
    [SerializeField] private InventoryCell[] _accessoryCells;
    [SerializeField] private Item _currentItem;

    private void Start()
    {
        _weaponCell.OnClick += ShowItem;
        _armorCell.OnClick += ShowItem;
        for(int i = 0; i < _accessoryCells.Length; i++)
        {
            _accessoryCells[i].OnClick += ShowItem;
        }
    }

    public void UpdateItems()
    {
        var inv = Player.Instance.Inventory;
        
        _nameText.text = "";
        _descriptionText.text = "";
        _unequipButton.gameObject.SetActive(false);
        
        _weaponCell.SetItem(inv.Weapon);
        _armorCell.SetItem(inv.Armor);
        for(int i = 0; i < _accessoryCells.Length && i < inv.Accessories.Count; i++)
        {
            _accessoryCells[i].SetItem(inv.Accessories[i]);
        }
    }

    private void ShowItem(Item item)
    {
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;
        _unequipButton.gameObject.SetActive(true);
        _currentItem = item;
    }
    public void UnequipItem()
    {
        Player.Instance.UnequipItem(_currentItem);
        UpdateItems();
    }
}

