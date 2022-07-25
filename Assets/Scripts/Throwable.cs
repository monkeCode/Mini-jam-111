using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float _moveTime;
    [SerializeField] private float _rotateTime;
    private Tween _moveTween;
    private Tween _rotateTween;
    void Start()
    {
       _moveTween = transform.DOMove(Player.Instance.transform.position, _moveTime).OnComplete(() => Destroy(gameObject));
       _rotateTween = transform.DORotate(new Vector3(0, 0, 359), _rotateTime).SetSpeedBased();
    }

    private void OnDestroy()
    {
        _moveTween.Kill();
        _rotateTween.Kill();
    }
}
