using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using Color = Dance.Color;

public class Mage : Entity, IDancer
{
    [SerializeField] private Ability _teleportAbility;
    [SerializeField] private Ability _fireSpinAbility;
    [SerializeField] private Ability _laserAbility;
    [SerializeField] private Animator _abilityAnimator;
    [SerializeField] private AudioClip _abilitySound;
    [SerializeField] private SpriteRenderer _shieldRenderer;
    private Animator _animator;
    private int _turn;
    protected override void Start()
    {
        base.Start();
        hitPoints = maxHitPoints;
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
        if (random <= 50)
        {
            Move();
        }
        else
        {
            UseAbility();
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
    private void UseAbility()
    {
        var playerPos = Player.Instance.transform.position;
        var distance = Vector2.Distance(transform.position, playerPos);
        if(distance < 3)
            _fireSpinAbility.Use(this, transform.position);
        else if(distance < 2)
            _teleportAbility.Use(this, transform.position);
        else
            _laserAbility.Use(this, transform.position);
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
