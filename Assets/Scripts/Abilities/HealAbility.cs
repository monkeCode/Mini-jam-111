using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/Heal", fileName = "HealAbility")]
public class HealAbility : Ability
{
    [SerializeField] private int heal;
    public override void Use(IDamageable target, Vector2 pos = default)
    {
        target.Heal((uint) heal);
    }
}
