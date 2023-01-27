using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityObservable
{
    public event Action<IDamageable, int> TakedDamage;
    public event Action<int> Healed; 
    public event Action<int,int> Moved;
    public event Action<IDamageable, int> Attacked;
    public event Action Died; 
}
