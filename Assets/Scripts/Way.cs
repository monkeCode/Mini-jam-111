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

    [SerializeField] private WayState _wayState;
    [SerializeField] private SpriteRenderer _doorRenderer;
    [SerializeField] private Transform _enterPosition;

    private void Start()
    {
        GameManager.Instance.ActiveRoom.AddTile(this);
    }

    public void OpenDoor()
    {
        if (_wayState != WayState.Blocked)
            _wayState = WayState.Open;
    }

    public void CloseDoor()
    {
        if (_wayState != WayState.Blocked)
            _wayState = WayState.Close;
    }
    public void Exit(Transform target)
    {
        if (_wayState == WayState.Open)
        {
            //exit
        }
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
