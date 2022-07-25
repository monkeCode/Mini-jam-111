using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Ability/Teleport")]
public class TeleportAbility : Ability
{
    public override void Use(IDamageable target)
    {
        var go = target as MonoBehaviour;
        if (go != null)
        {
           var pos = GameManager.Instance.ActiveRoom.RandomValidPosition();
           go.transform.position = pos;
        }
    }
}
