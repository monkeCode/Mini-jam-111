using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Entity
{
    private int _stepper = 0;
    protected override void MoveToTile(Vector2 pos)
    {
        transform.position = pos;
    }
    
    public override void NextTurn()
    {
        if (_stepper == 0)
        {
            base.NextTurn();
            _stepper++;
            return;
        }
        _stepper = 0;

    }
}
