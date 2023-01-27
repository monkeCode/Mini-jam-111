using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Star : MonoBehaviour
{
    [SerializeField] private float _timeSpeed = 3.0f;
    private int _damage;
    public void Init(Vector2 pos, int damage)
    {
        _damage = damage;
        transform.position = pos - (Vector2.up + Vector2.right * Random.Range(-0.5f,0.5f)).normalized * 15;
        transform.DOMove(pos, _timeSpeed).OnComplete(Explosion) ;
    }

    private void Explosion()
    {
        GetComponent<Animator>()?.SetTrigger("Explose");
        var enemy = GameManager.Instance.ActiveRoom
            .GetAllEntities().FirstOrDefault(it => it.transform.position == transform.position);
        
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
    
}
