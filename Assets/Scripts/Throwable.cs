using System;
using DG.Tweening;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float _moveTime;
    [SerializeField] private float _rotateSpeed;
    private Tween _moveTween;
    private Tween _rotateTween;
    [SerializeField] private Transform _target;
    [SerializeField] private uint damage;
   
    
    public void Init(int damage, Transform target)
    {
        _target = target;
        this.damage = (uint)damage;
        _moveTween = transform.DOMove(_target.position, _moveTime).OnComplete(OnComplete);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnComplete()
    {
        _target.GetComponent<IDamageable>()?.TakeDamage(damage);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        _moveTween.Kill();
        _rotateTween.Kill();
    }
}
