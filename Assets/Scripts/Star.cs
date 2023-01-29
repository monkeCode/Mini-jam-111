using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class Star : MonoBehaviour
{
    [SerializeField] private float _timeSpeed = 3.0f;
    private int _damage;
    public void Init(Transform pos, int damage)
    {
        _damage = damage;
        transform.position = (Vector2)pos.position - (Vector2.down + Vector2.right * Random.Range(-0.5f,0.5f)).normalized * 15;
        StartCoroutine(Move(pos));
    }

    IEnumerator Move(Transform pos)
    {
        
        while (pos != null && transform.position != pos.position )
        {
            transform.position = Vector2.MoveTowards(transform.position, pos.position, _timeSpeed * Time.deltaTime);
            yield return null;
        }
        Explosion();
    }

    private void Explosion()
    {
        GetComponent<Animator>()?.SetTrigger("Explose");
        var enemy = GameManager.Instance.ActiveRoom
            .GetAllEntities().FirstOrDefault(it => it.transform.position == transform.position);
        enemy.TakeDamage((uint) _damage);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
    
}
