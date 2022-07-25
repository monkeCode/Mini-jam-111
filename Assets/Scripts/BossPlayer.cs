using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = Dance.Color;

public class BossPlayer : Entity, IDancer
{
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private Animator _abilityAnimator;
    private Animator _animator;
    protected override void Start()
    {
        base.Start();
        maxHitPoints = Player.Instance.MaxHitPoints;
        hitPoints = maxHitPoints;
        _abilities = Player.Instance.Abilities.ToList();
        _animator = GetComponent<Animator>();
    }

    public void AddColor(Color color)
    {
        
    }

    public override void NextTurn()
    {
        var random = Random.Range(1, 101);
        if (random <= 10)
        {
            return;
        }
        else if (random <= 80)
        {
            Move();
        }
        else
        {
            UseRandomAbility();
        }
    }

    public override void Move()
    {
        base.Move();
        GameManager.Instance.ActiveRoom.GetFloorTile(transform.position).Step(transform);
    }
    public override void Heal(uint healp)
    {
        base.Heal(healp);
        _abilityAnimator.SetTrigger("Heal");
    }
    public override void TakeDamage(uint damage)
    {
        if (hitPoints > 0)
            hitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        if (hitPoints <= 0)
            Die();
    }
    private void UseRandomAbility()
    {
        var random = Random.Range(0, _abilities.Count);
        _abilities[random].Use(this);
    }

    protected override void Die()
    {
        base.Die();
        UserInterface.Instance.ShowWinPanel();
    }
}
