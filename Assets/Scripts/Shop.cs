using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shop : Room
{
    [SerializeField] private ShopTile[] _shopTiles;
    [SerializeField] private Ability[] _sellableItems;
    private void Start()
    {
        foreach (var tile in _shopTiles)
        {
            AddTile(tile);
        }
        var items = _sellableItems.Except(Player.Instance.Abilities).ToList();
        for (int i = 0; i < _shopTiles.Length && items.Count > 0; i++)
        {
            var it = items[Random.Range(0, items.Count)];
            _shopTiles[i].SetItem(it);
            items.Remove(it);
        }
    }
}
