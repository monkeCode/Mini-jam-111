using System.Linq;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Ability/Laser", fileName = "LaserAbility")]
    public class LaserAbility : Ability
    {
        [SerializeField] private uint _damage;
        [SerializeField] private GameObject _laserPrefab;
        public override void Use(IDamageable target, Vector2 pos = default)
        {
            if (target != Player.Instance)
            {
                if(Player.Instance.transform.position.x == pos.x || Player.Instance.transform.position.y == pos.y)
                    Player.Instance.TakeDamage(_damage);
            }
            else
            {
                var entities = GameManager.Instance.ActiveRoom.GetAllEntities();
                entities = entities.Where(en => en.transform.position.x == pos.x || en.transform.position.y == pos.y).ToList();
                foreach (var entity in entities)
                {
                    entity.TakeDamage(_damage);
                }
            }
            var laser = Instantiate(_laserPrefab, pos, Quaternion.identity);
            Destroy(laser,1f);
        }
    }
}
