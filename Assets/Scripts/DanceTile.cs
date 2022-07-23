using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dance{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DanceTile : MonoBehaviour, IFloorTile
{
    [SerializeField] private Color _color;
    [SerializeField] private int _updateTurns;
    private int _turnNow;
    private SpriteRenderer _sprite;
    public bool ActiveTile => _turnNow <= 0;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        RandomColor();
        GameManager.Instance.ActiveRoom.AddTile(this);
    }
    
    [ContextMenu("RandomColor")]
    private void RandomColor()
    {
        _color = (Color) Random.Range(1, 5);
        _sprite ??= GetComponent<SpriteRenderer>();
        _sprite.color = GameManager.Colors[_color];

    }
    public void NextTurn()
    {
        _turnNow--;
        _sprite.color = !ActiveTile ? UnityEngine.Color.grey : GameManager.Colors[_color];
    }

    public Vector2 Position => transform.position;

    public bool CanStep => true;
    public void Step(Transform target)
    {
        if (!ActiveTile) return;
        if(target.TryGetComponent(out IDancer dancer))
        {
            dancer.AddColor(_color);
            _turnNow = _updateTurns+1;
        }
    }
}
}