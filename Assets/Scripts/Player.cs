using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dance;
using UnityEngine;
using Color = Dance.Color;

public class Player : MonoBehaviour, IDamageable
{
    private GameInput _input;
    private Dance.Color[] _colorSequence = new Dance.Color[8];
    [SerializeReference] private List<Ability> _abilities;
    
   [SerializeField] private int _hitPoints;
   [SerializeField] private int _maxHitPoints;
    public int HitPoints => _hitPoints;
    public int MaxHitPoints => _maxHitPoints;
    void Start()
    {
        _input = new GameInput();
        _input.Enable();
        _input.Player.Move.performed += context => Move(context.ReadValue<Vector2>());
        _input.Player.Sumbit.performed += context => UseAbility();
    }

    private void Move(Vector2 dir)
    {
        if (Math.Abs(dir.x) < 1 && Math.Abs(dir.y) < 1)
            return;
        if (GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + dir))
        {
            transform.Translate(dir);
            var danceTile = GameManager.Instance.ActiveRoom.GetFloorTile(transform.position);
            if (danceTile != null)
            {
                var color = danceTile.Use();
                if (color != Color.Null)
                {
                    AddColor(color);
                }
                    
            }

            GameManager.Instance.NextTurn();
        }
    }

    private void AddColor(Dance.Color color)
    {
        for (int i = 0; i < _colorSequence.Length; i++)
        {
            if (_colorSequence[i] == Color.Null)
            {
                _colorSequence[i] = color;
                return;
            }
        }

        for (int i = _colorSequence.Length-1; i >0; i--)
        {
            _colorSequence[i] = _colorSequence[i - 1];
        }

        _colorSequence[0] = color;
        // foreach (var col in _colorSequence)
        // {
        //     Debug.Log(col);
        // }
    }

    private void UseAbility()
    {
        using var sequences= _abilities.OrderByDescending(ab => ab.Sequence.Count).GetEnumerator();
        while (sequences.MoveNext())
        {
            var index = FindSubArrayIndex(sequences.Current.Sequence);
            if(index != -1)
                sequences.Current.Use(this);
            for (int i = index; i < sequences.Current.Sequence.Count + index; i++)
            {
                _colorSequence[i] = Color.Null;
            }
        }
    }

    private int FindSubArrayIndex(IReadOnlyList<Dance.Color> sequence)
    {
        for (int i = 0; i < _colorSequence.Length - sequence.Count + 1; i++)
        {
            var index = i;
            for (int j = 0; j < sequence.Count; j++)
            {
                if (_colorSequence[i + j] != sequence[j])
                {
                    index = -1;
                    break;
                }
            }
            if (index >= 0) 
                return index;
        }
        return -1;
    }
    

    public void Heal(uint healp)
    {
        _hitPoints += (int)healp;
        if (_hitPoints > _maxHitPoints)
            _hitPoints = _maxHitPoints;
    }

    public void Kill()
    {
        TakeDamage((uint)_hitPoints);
    }


    public void TakeDamage(uint damage)
    {
        if (_hitPoints > 0)
            _hitPoints -= (int)damage;
        if (_hitPoints <= 0)
            Die();
    }

    private void Die()
    {
    }
}
