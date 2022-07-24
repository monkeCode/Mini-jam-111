using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour,IFloorTile
{
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
    private void Start()
    {
        GameManager.Instance.ActiveRoom.AddTile(this);
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
        }
    }

    public void BlockDoor()
    {
        _wayState = WayState.Blocked;
    }
    public bool CanStep => _wayState == WayState.Open;
    public void Step(Transform target)
    {
        Exit(target);
    }

    public void NextTurn()
    {
        
    }

    public Vector2 Position => transform.position;
}
