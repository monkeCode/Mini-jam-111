using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] private List<Dance.Color> _sequence;
    [SerializeField] private string _name;
    public string Name => _name;
    public IReadOnlyList<Dance.Color> Sequence => _sequence;
    public abstract void Use(IDamageable target);
}
