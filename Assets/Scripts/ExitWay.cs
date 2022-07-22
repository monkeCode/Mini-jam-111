using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ExitWay : MonoBehaviour
{
   [SerializeField] private Way way;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent(out Player player))
      {
         way.Exit();
      }
   }
}
