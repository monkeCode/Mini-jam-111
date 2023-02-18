using System;
using UnityEngine;

public class SpikeTile : MonoBehaviour, IFloorTile
{
    public bool CanStep => true;
    public uint StepCost => _state == 1?_damage:2;
    [SerializeField] private uint _damage;
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    private int _state;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.ActiveRoom.AddTile(this);
    }

    public void Step(Transform target)
    {
        if (_state == 2 && target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
       
    }

    public void NextTurn()
    {
        _state = (_state + 1) % 4;
        _spriteRenderer.sprite = _sprites[_state];
    }

    public Vector2 Position => transform.position;
}
