using UnityEngine;
[CreateAssetMenu(fileName = "Shield Ability", menuName = "Ability/Shield")]
public class ShieldAbility : Ability
{
    [SerializeField] private Shield _shield;

    public override void Use(IDamageable target,Vector2 pos = default)
    {
        target.AddShield(Instantiate(_shield));
    }
}
