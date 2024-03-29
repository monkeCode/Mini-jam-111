using System.Linq;
using Dance;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Ability/Shuffle")]
    public class ShuffleAbility : Ability
    {
        public override void Use(IDamageable target, Vector2 pos = default)
        {
            GameManager.Instance.ActiveRoom.Tiles.Select(it => it as DanceTile).
                Where(it => it != null).ToList().
                ForEach(it=> it.RandomColor());
        }
    }
}
