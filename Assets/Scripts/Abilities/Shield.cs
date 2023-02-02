using UnityEngine;

namespace Abilities
{
   [CreateAssetMenu(menuName = "Shield/Base")]
   public class Shield:ScriptableObject,ITurned
   {
      [SerializeField] private int _turns;
      [SerializeField] private UnityEngine.Color _color;
      public UnityEngine.Color Color => _color;
      public bool Active => _turns > 0;
      public void NextTurn()
      {
         _turns--;
      }

      public virtual uint Defence(IDamageable target, uint damage)
      {
         return Active ? 0 : damage;
      }
   }
}
