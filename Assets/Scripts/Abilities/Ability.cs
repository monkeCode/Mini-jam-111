using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : ScriptableObject, ISellableItem
    {
        [SerializeField] private List<Dance.Color> _sequence;
        [SerializeField] private string _name;
        [SerializeField][Min(0)] private int _cost;
        public string Name => _name;
        public IReadOnlyList<Dance.Color> Sequence => _sequence;
        public abstract void Use(IDamageable target, Vector2 pos = default);
        public int Cost => _cost;
        public void Buy(Player player)
        {
            player.AddAbility(Instantiate(this));
        }
    
        public static bool operator==(Ability first, Ability second)
        {
            if (first is null || second is null)
            {
                if(first is null && second is null)
                    return true;
                return false;
            }
            return first._name == second._name;
        }

        public static bool operator !=(Ability first, Ability second)
        {
            return !(first == second);
        }

        public override string ToString() => Name;
    }
}
