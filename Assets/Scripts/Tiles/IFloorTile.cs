using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorTile
{
    public bool CanStep { get; }
    public uint StepCost { get; }
    public void Step(Transform target);
    public void NextTurn();
    public Vector2 Position { get; }
}
