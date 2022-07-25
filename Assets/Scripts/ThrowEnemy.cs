using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEnemy : Entity
{
    private Animator _animator;
    [SerializeField] GameObject _thowableObject;
    [SerializeField] private int _lookDistance;
    private int _atkStack = 0;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void Move()
    {
        var playerPos = Player.Instance.transform.position;
        var horizontalStack = GameManager.Instance.ActiveRoom.MoveToPos(transform.position, new Vector2(playerPos.x, transform.position.y));
        var verticalStack = GameManager.Instance.ActiveRoom.MoveToPos(transform.position, new Vector2(transform.position.x, playerPos.y));
        Vector2 nextMove = Vector2.zero;
        if((horizontalStack == null && verticalStack == null) || _lookDistance > Math.Max(horizontalStack?.Count??0, verticalStack?.Count??0))
        {
            base.Move();
            return;
        }
        if(horizontalStack == null)
        {
            nextMove = verticalStack.Pop();
        }
        else if(verticalStack == null)
        {
            nextMove = horizontalStack.Pop();
        }
        else
        {
            if(horizontalStack.Count < verticalStack.Count)
            {
                nextMove = horizontalStack.Pop();
            }
            else
            {
                nextMove = verticalStack.Pop();
            }
        }
        transform.position += (Vector3)nextMove;
    }

    public override void NextTurn()
    {
        var playerPos = Player.Instance.transform.position;
        if(transform.position.x == playerPos.x || transform.position.y == playerPos.y)
        {
           Attack();
        }
        else
        {
            _atkStack = 0;
            Move();
        }
    }
    public override void Attack()
    {
        if (_atkStack < 2)
        {
            _atkStack++;
            return;
        }

        _atkStack = 0;
        Instantiate(_thowableObject, transform.position, Quaternion.identity);
        Player.Instance.TakeDamage((uint) damage);
    }
    
    public override void TakeDamage(uint damage)
    {
        if (hitPoints > 0)
            hitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        if (hitPoints <= 0)
            Die();
    }
}
