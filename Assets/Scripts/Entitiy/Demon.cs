using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Demon : Entity
{
    private Animator _animator;

    private bool IsAttack
    {
        get => _animator.GetBool("IsAttack");
        set => _animator.SetBool("IsAttack", value);
    }

    private Vector2 lastPlayerPos;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void Move()
    {
        var playerPos = Player.Instance.transform.position;
        if (IsAttack)
        {
            if (lastPlayerPos ==(Vector2)playerPos)
            {
                Attack();
            }
            else
            {
                if(GameManager.Instance.ActiveRoom.GetAllEntities().Where(it => (Vector2)it.transform.position == lastPlayerPos).ToList().Count == 0)
                    transform.position = lastPlayerPos;
            }

            IsAttack = false;
            return;
        }
        var stack = GameManager.Instance.ActiveRoom.MoveToPos(transform.position, playerPos);
        if(stack == null)
            return;
        if(stack.Count > 1)
            transform.position = (Vector2)transform.position + stack.Pop();
        else
        {
            IsAttack = true;
            lastPlayerPos = playerPos;
        }
    }

    public override void Attack()
    {
        Player.Instance.TakeDamage((uint) damage, this);
        PlaySound(hitSound);
    }
    public override void TakeDamage(uint damage, IDamageable source = null)
    {
        if (hitPoints > 0)
            hitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        if (hitPoints <= 0)
            Die();
    }
}
