using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Entity : MonoBehaviour, IDamageable, ITurned
{
  [SerializeField] protected int hitPoints;
  [SerializeField] protected int maxHitPoints;
  [SerializeField] protected int damage;
  [SerializeField] protected AudioClip hitSound;
  [SerializeField] protected Shield activeShield;
  public int HitPoints => hitPoints;
  public int MaxHitPoints => maxHitPoints;
  protected AudioSource audioSource;

  public virtual void Heal(uint healp)
  {
      hitPoints += (int)healp;
      if (hitPoints > maxHitPoints)
          hitPoints = maxHitPoints;
  }


  public virtual void TakeDamage(uint damage, IDamageable source = null)
  {
      if(activeShield != null)
        damage = activeShield.Defence(this,damage);
      if (hitPoints > 0)
          hitPoints -= (int)damage;
      if (hitPoints <= 0)
            Die();
  }

  protected virtual void Start()
  {
      GameManager.Instance.ActiveRoom.AddEntity(this);
        audioSource = GetComponent<AudioSource>();
  }

  public void Kill()
    {
        TakeDamage((uint)hitPoints);
    }

  public virtual void AddShield(Shield shield)
  {
      activeShield = shield;
  }

  protected virtual void Die()
    {
        GameManager.Instance.ActiveRoom.RemoveEntity(this);
        Destroy(gameObject);
    }

    public virtual void NextTurn()
    {
        Move();
    }

    public virtual void Attack()
    {
        AttackAnimation(Player.Instance.transform);
        Player.Instance.TakeDamage((uint) damage, this);
        PlaySound(hitSound);
    }

    protected void AttackAnimation(Transform pos)
    {
        var startPos = transform.position;
        transform.DOMove((pos.position+ startPos)/2, 0.15f).OnComplete(() => transform.DOMove(startPos, 0.15f));
    }

    public virtual void Move()
    {
        var playerPos = Player.Instance.transform.position;
        var stack = GameManager.Instance.ActiveRoom.MoveToPos(transform.position, playerPos);
        if(stack == null)
            return;
        if (stack.Count > 1)
        {
           MoveToTile(stack.Pop() + (Vector2)transform.position);
        }
        else
        {
            Attack();
        }
    }
    protected void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    protected virtual void MoveToTile(Vector2 pos)
    {
        transform.position = pos;
        GameManager.Instance.ActiveRoom.GetFloorTile(pos).Step(transform);
    }
}
