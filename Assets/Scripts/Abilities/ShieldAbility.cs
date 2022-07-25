using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Shield Ability", menuName = "Ability/Shield")]
public class ShieldAbility : Ability
{
    [SerializeField] private Shield _shield;

    public override void Use(IDamageable target)
    {
        target.AddShield(Instantiate(_shield));
    }
}
