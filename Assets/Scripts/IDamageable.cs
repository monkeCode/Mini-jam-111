using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
public interface IDamageable
{
    public int HitPoints {get; }
    public int MaxHitPoints { get; }
    public void TakeDamage(uint damage, IDamageable source = null);
    public void Heal(uint healp);
    public void Kill();
    public void AddShield(Shield shield);
}
