using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dance;
using UnityEngine;
using Color = Dance.Color;

public class Player : MonoBehaviour, IDamageable, IDancer
{
    private GameInput _input;
    private Dance.Color[] _colorSequence = new Dance.Color[8];
    [SerializeReference] private List<Ability> _abilities;
    [SerializeField] private int _hitPoints;
   [SerializeField] private int _maxHitPoints;
   [SerializeField] private int damage;
   [SerializeField] private Animator _healAnimator;
   private Animator _animator;
    public int HitPoints => _hitPoints;
    public int MaxHitPoints => _maxHitPoints;
    public IReadOnlyList<Ability> Abilities => _abilities;
    public static Player Instance { get; private set; }
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _input = new GameInput();
        _input.Enable();
        _input.Player.Move.performed += context => Move(context.ReadValue<Vector2>());
        _input.Player.Sumbit.performed += context => UseAbility();
        _animator = GetComponent<Animator>();
    }

    private void Move(Vector2 dir)
    {
        if (Math.Abs(dir.x) < 1 && Math.Abs(dir.y) < 1)
            return;
        var entity = GameManager.Instance.ActiveRoom.GetAllEntities().FirstOrDefault(en => (Vector2)en.transform.position == (Vector2)transform.position + dir);
        if (entity != null)
        {
            Attack(entity);
        }
        else if (GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + dir, out IFloorTile tile ))
        {
            transform.position += (Vector3)dir;
            tile.Step(transform);
        }
        else
        {
            return;
        }
        GameManager.Instance.NextTurn();
    }

    private void Attack(Entity entity)
    {
        entity.TakeDamage((uint) damage);
    }

    public void AddColor(Dance.Color color)
    {
        // for (int i = 0; i < _colorSequence.Length; i++)
        // {
        //     if (_colorSequence[i] == Color.Null)
        //     {
        //         _colorSequence[i] = color;
        //         return;
        //     }
        // }

        for (int i = _colorSequence.Length-1; i > 0; i--)
        {
            _colorSequence[i] = _colorSequence[i - 1];
        }

        _colorSequence[0] = color;
        // foreach (var col in _colorSequence)
        // {
        //     Debug.Log(col);
        // }
        UserInterface.Instance.UpdateSequence(_colorSequence);
    }

    private void UseAbility()
    {
        using var sequences= _abilities.OrderByDescending(ab => ab.Sequence.Count).GetEnumerator();
        while (sequences.MoveNext())
        {
            var index = FindSubArrayIndex(sequences.Current.Sequence);
            if (index != -1)
            {
                sequences.Current.Use(this);
                for (int i = index; i < sequences.Current.Sequence.Count + index; i++)
                {
                    _colorSequence[i] = Color.Null;
                }
            }
        }
        UserInterface.Instance.UpdateSequence(_colorSequence);
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

    public void AddAbility(Ability ability)
    {
        _abilities.Add(ability);
    }

    public void Heal(uint healp)
    {
        _hitPoints += (int)healp;
        if (_hitPoints > _maxHitPoints)
            _hitPoints = _maxHitPoints;
        UserInterface.Instance.UpdateHpBar();
        _healAnimator.SetTrigger("Heal");
    }

    public void Kill()
    {
        TakeDamage((uint)_hitPoints);
    }


    public void TakeDamage(uint damage)
    {
        if (_hitPoints > 0)
            _hitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        UserInterface.Instance.UpdateHpBar();
        if (_hitPoints <= 0)
            Die();
    }

    private void Die()
    {
    }

    public void CollectMoney(uint money)
    {
        //TODO:collect money
    }
}
