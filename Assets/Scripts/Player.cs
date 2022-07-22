using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private Room _test;
    private GameInput _input;
    void Start()
    {
        _input = new GameInput();
        _input.Enable();
        _input.Player.Move.performed += context => Move(context.ReadValue<Vector2>());
    }

    private void Move(Vector2 dir)
    {
        if(Math.Abs(dir.x) <1 && Math.Abs(dir.y) < 1)
            return;
        if (_test.CanMove((Vector2) transform.position + dir))
        {
            transform.Translate(dir);
            var danceTile = _test.GetFloorTile(transform.position);
            if(danceTile != null)
                danceTile.Use();
        }
    }
    void Update()
    {
        
    }
}
