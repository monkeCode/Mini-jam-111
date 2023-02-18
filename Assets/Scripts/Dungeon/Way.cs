using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Way : MonoBehaviour,IFloorTile
{
    private AudioSource _audioSource;
    public enum WayState
    {
        Open,
        Close,
        Blocked
    }

    public Direction Dir;
    public enum Direction
    {
        Left,Top,Right,Bottom
    }
    [SerializeField] private WayState _wayState;
    [SerializeField] private SpriteRenderer _doorRenderer;
    [SerializeField] private Transform _enterPosition;
    [SerializeField] private Sprite[] _doorSprite;
    public Vector2 EnterPonit => _enterPosition.position;
    public WayState State => _wayState;
    private void Start()
    {
        GameManager.Instance.ActiveRoom.AddTile(this);
        _audioSource = GetComponent<AudioSource>();
        
    }

    public void OpenDoor()
    {
        if (_wayState != WayState.Blocked)
        {
            _wayState = WayState.Open;
            _doorRenderer.sprite = _doorSprite[0];
        }
    }

    public void CloseDoor()
    {
        if (_wayState != WayState.Blocked)
        {
            _wayState = WayState.Close;
            _doorRenderer.sprite = _doorSprite[1];
        }
        
    }
    public void Exit(Transform target)
    {
        if (_wayState == WayState.Open)
        {
            GameManager.Instance.ActiveRoom.Exit(this);
            _audioSource.Play();
        }
    }

    public void BlockDoor()
    {
        _wayState = WayState.Blocked;
    }
    public void UnblockDoor()
    {
        _wayState = WayState.Close;
    }
    public bool CanStep => _wayState == WayState.Open;
    public uint StepCost => 1;

    public void Step(Transform target)
    {
        if(target.TryGetComponent(out Player player))
            Exit(target);
    }

    public void NextTurn()
    {
        
    }

    public Vector2 Position => transform.position;
}
