using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopTile : MonoBehaviour, IFloorTile
{

    public bool CanStep => _isSold || Player.Instance.Coins >= _cost;
    private ISellableItem _item;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Transform _itemPresenter;
    private int _cost;
    private bool _isSold;
    
    public Vector2 Position => transform.position;
    
    public void Step(Transform target)
    {
        if(_isSold || _item == null) return;
        var player = target.GetComponent<Player>();
        if(player == null) return;
        _item.Buy(player);
        player.Coins -= _cost;
        _isSold = true;
        _itemPresenter.gameObject.SetActive(false);
        _text.text = "Sold";
    }

    public void NextTurn()
    {
        
    }

    public void SetItem(ISellableItem item)
    {
        _item = item;
        if (_item == null)
        {
            _itemPresenter.gameObject.SetActive(false);
            _text.text = "";
            return;
        }
        _cost = (int) (Random.Range(0.9f, 1.1f) * _item.Cost);
        _text.text = _item + $"\nCost:{_cost}";
    }

    public void SetIcon(Sprite sprite)
    {
        _itemPresenter.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
