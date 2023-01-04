using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Ability : ScriptableObject, ISellableItem
{
    [SerializeField] private List<Dance.Color> _sequence;
    [SerializeField] private string _name;
    [SerializeField][Min(0)] private int _cost;
    public string Name => _name;
    public IReadOnlyList<Dance.Color> Sequence => _sequence;
    public abstract void Use(IDamageable target);
    public int Cost => _cost;
    public void Buy(Player player)
    {
        player.AddAbility(Instantiate(this));
    }
    
    public static bool operator==([NotNull] Ability first, [NotNull] Ability second)
    {
        return first._name == second._name;
    }

    public static bool operator !=(Ability first, Ability second)
    {
        return !(first == second);
    }

    public override string ToString() => Name;
}
