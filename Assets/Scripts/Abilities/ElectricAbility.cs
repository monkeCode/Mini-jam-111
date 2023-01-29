using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/FireSpin",fileName = "FireAbility")]
public class ElectricAbility : Ability
{
    [SerializeField] private int _radius;
    [SerializeField] private int damage;
    [SerializeField] private GameObject effect;

    public override void Use(IDamageable target, Vector2 pos = default)
    {
        if (target == Player.Instance)
        {
            var entities = GameManager.Instance.ActiveRoom.GetAllEntities()
                .Where(en => Vector2.Distance(en.transform.position, pos) <= _radius);
            foreach (var en in entities)
            {
                en.TakeDamage((uint) damage);
            }
        }
        else
        {
            if(Vector2.Distance(pos, Player.Instance.transform.position) <= _radius)
                Player.Instance.TakeDamage((uint) damage);
        }

        var scale = effect.transform.localScale.x;
        Instantiate(effect, pos, Quaternion.identity).transform.localScale = new Vector3(_radius* scale, _radius* scale, 1);
    }
}
