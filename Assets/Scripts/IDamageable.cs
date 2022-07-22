using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamageable
{
    public int HitPoints {get; }
    public int MaxHitPoints { get; }
    public void TakeDamage(uint damage);
    public void Heal(uint healp);
    public void Kill();
}