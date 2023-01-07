using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Color = Dance.Color;

public class EventText : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI _textGui;
    [SerializeField] private float _speed;
    private string _text = "Test Text";

    private void Start()
    {
        _textGui.text = _text;
        transform.DOMove(Vector3.up, _speed).SetSpeedBased();
        _textGui.DOFade(0, _speed).OnComplete(() => Destroy(gameObject));
    }

    public void Init(string text)
    {
        _text = text;
    }
}
