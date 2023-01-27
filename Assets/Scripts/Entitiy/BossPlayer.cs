using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = Dance.Color;

public class BossPlayer : Entity, IDancer
{
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private Animator _abilityAnimator;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private AudioClip _abilitySound;
    [SerializeField] private SpriteRenderer _shieldRenderer;
    private Animator _animator;
    private int _turn;
    protected override void Start()
    {
        base.Start();
        maxHitPoints = (int)(Player.Instance.MaxHitPoints*1.5f);
        hitPoints = maxHitPoints;
        _abilities = Player.Instance.Abilities.ToList();
        _animator = GetComponent<Animator>();
    }

    public void AddColor(Color color)
    {
        
    }

    public override void NextTurn()
    {
        if (activeShield != null)
        {
            activeShield.NextTurn();
            _shieldRenderer.enabled = activeShield.Active;
        }

        if (_turn == 1)
        {
            _turn = 0;
            return;
        }

        _turn++;
        var random = Random.Range(1, 101);
        if (random <= 70)
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
    public override void TakeDamage(uint damage, IDamageable source = null)
    {
        if (activeShield != null)
            damage = activeShield.Defence(this,damage);
        if (hitPoints > 0)
            hitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        if (hitPoints <= 0)
            Die();
    }
    private void UseRandomAbility()
    {
        if(_abilities.Count == 0)
            return;
        var random = Random.Range(0, _abilities.Count);
        _abilities[random].Use(this);
        PlaySound(_abilitySound);
    }

    protected override void Die()
    {
        base.Die();
        UserInterface.Instance.ShowWinPanel();
    }
    public override void AddShield(Shield shield)
    {
        activeShield = shield;
        _shieldRenderer.enabled = activeShield.Active;
        _shieldRenderer.color = activeShield.Color;
    }
}
