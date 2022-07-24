using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/FireSpin",fileName = "FireAbility")]
public class ElectricAbility : Ability
{
    [SerializeField] private int countOfEntities;
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;

    public override void Use(IDamageable target)
    {
        var entityList = GameManager.Instance.ActiveRoom.GetAllEntities().Take(countOfEntities);
        foreach (var ent in entityList)
        {
            ent.TakeDamage((uint) damage);
            Instantiate(effect, ent.transform.position, Quaternion.identity);
        }

    }
}
