using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
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
    public void Exit()
    {
        if (_wayState == WayState.Open)
        {
            //exit
        }
    }
}
