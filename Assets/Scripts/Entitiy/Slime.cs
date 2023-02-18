using System.Collections;
using System.Collections.Generic;
using Dance;
using UnityEngine;

public class Slime : Entity
{
    [SerializeField] private Dance.Color _color;
    
    private int _delay = 0;
    public override void Move()
    {
        if (_delay == 1)
        {
            _delay = 0;
            return;
        }

        _delay = 1;
        while (true)
        {
            var directions = new[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
            var direction = directions[Random.Range(0, 3)];

            if (transform.position + (Vector3) direction == Player.Instance.transform.position)
            {
                Attack();
                return;
            }

            if (!GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + direction, out var tile))
                continue;
            var pos = (Vector2)transform.position + direction;
            MoveToTile(pos);
            if (tile is DanceTile t)
            {
                t.SetColor(_color);
            }
            return;
        }
    }
}
