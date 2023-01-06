using UnityEngine;
using Random = UnityEngine.Random;

public class Shop : Room
{
    [SerializeField] private ShopTile[] _shopTiles;
    private void Start()
    {
        foreach (var tile in _shopTiles)
        {
            AddTile(tile);
        }

        foreach (var t in _shopTiles)
        {
            if (Random.Range(0, 2) == 0)
            {
                var item = GameManager.Instance.GetRandomSpell();
                t.SetItem(item);
            }
            else
            {
                var item = GameManager.Instance.GetRandomItem();
                t.SetItem(item);
                if(item != null)
                    t.SetIcon(item.Icon);
            }
        }
    }
}
