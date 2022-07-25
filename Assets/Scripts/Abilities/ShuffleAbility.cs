using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dance;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/Shuffle")]
public class ShuffleAbility : Ability
{
    public override void Use(IDamageable target)
    {
        GameManager.Instance.ActiveRoom.Tiles.Select(it => it as DanceTile).
            Where(it => it != null).ToList().
            ForEach(it=> it.RandomColor());
    }
}
