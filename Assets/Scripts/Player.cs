using System;
using System.Collections;
using System.Collections.Generic;
using Dance;
using UnityEngine;
using Color = Dance.Color;

public class Player : MonoBehaviour
{
    private GameInput _input;
    private Dance.Color[] _colorSequence = new Dance.Color[8];
    void Start()
    {
        _input = new GameInput();
        _input.Enable();
        _input.Player.Move.performed += context => Move(context.ReadValue<Vector2>());
    }

    private void Move(Vector2 dir)
    {
        if (Math.Abs(dir.x) < 1 && Math.Abs(dir.y) < 1)
            return;
        if (GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + dir))
        {
            transform.Translate(dir);
            var danceTile = GameManager.Instance.ActiveRoom.GetFloorTile(transform.position);
            if (danceTile != null)
            {
                var color = danceTile.Use();
                if (color != Color.Null)
                {
                    AddColor(color);
                }
                    
            }

            GameManager.Instance.NextTurn();
        }
    }

    private void AddColor(Dance.Color color)
    {
        for (int i = 0; i < _colorSequence.Length; i++)
        {
            if (_colorSequence[i] == Color.Null)
            {
                _colorSequence[i] = color;
                return;
            }
        }

        for (int i = _colorSequence.Length-1; i >0; i--)
        {
            _colorSequence[i] = _colorSequence[i - 1];
        }

        _colorSequence[0] = color;
        // foreach (var col in _colorSequence)
        // {
        //     Debug.Log(col);
        // }
    }
}
