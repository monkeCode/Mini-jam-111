using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable, ITurned
{
  [SerializeField] protected int hitPoints;
  [SerializeField] private int _maxHitPoints;
  public int HitPoints => hitPoints;
  public int MaxHitPoints => _maxHitPoints;

  public void Heal(uint healp)
  {
      hitPoints += (int)healp;
      if (hitPoints > _maxHitPoints)
          hitPoints = _maxHitPoints;
  }


  public void TakeDamage(uint damage)
    {
        if (hitPoints > 0)
            hitPoints -= (int)damage;
        if (hitPoints <= 0)
            Die();
    }

  protected void Start()
  {
      GameManager.Instance.ActiveRoom.AddEntity(this);
  }

  public void Kill()
    {
        TakeDamage((uint)hitPoints);
    }

    protected virtual void Die()
    {
        GameManager.Instance.ActiveRoom.RemoveEntity(this);
        Destroy(gameObject);
    }

    public void NextTurn()
    {
        
    }
}
