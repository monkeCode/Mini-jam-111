using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dance{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DanceTile : MonoBehaviour, ITurned
{
    [SerializeField] private Color _color;
    [SerializeField] private int _updateTurns;
    private int _turnNow;
    private SpriteRenderer _sprite;
    public bool ActiveTile { get; private set; }
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        RandomColor();
    }
    
    [ContextMenu("RandomColor")]
    private void RandomColor()
    {
        _color = (Color) Random.Range(0, 5);
        _sprite ??= GetComponent<SpriteRenderer>();
        _sprite.color = GameManager.Colors[_color];

    }
    public void Use()
    {
        _turnNow = _updateTurns;
    }
    public void NextTurn()
    {
        ActiveTile = --_turnNow <= 0;
    }
    
}
}