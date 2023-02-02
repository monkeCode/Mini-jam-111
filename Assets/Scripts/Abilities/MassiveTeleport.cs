using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "MassiveTeleport", menuName = "Ability/MassiveTeleport")]
    public class MassiveTeleport : Ability
    {
        public override void Use(IDamageable target, Vector2 pos = default)
        {
            var entities = GameManager.Instance.ActiveRoom.GetAllEntities();
            foreach (var entity in entities)
            {
                entity.transform.position = GameManager.Instance.ActiveRoom.RandomValidPosition();
            }
            Player.Instance.transform.position = GameManager.Instance.ActiveRoom.RandomValidPosition();
        }
    }
}
