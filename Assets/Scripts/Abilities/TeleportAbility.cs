using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/Teleport")]
public class TeleportAbility : Ability
{
    public override void Use(IDamageable target, Vector2 position = default)
    {
        var go = target as MonoBehaviour;
        if (go != null)
        {
           var pos = GameManager.Instance.ActiveRoom.RandomValidPosition();
           go.transform.position = pos;
        }
    }
}
