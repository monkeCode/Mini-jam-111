using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(SpriteRenderer))]
public class LootChest : MonoBehaviour, IFloorTile
{
    [SerializeField] private List<Ability> _abilityList;
    [SerializeField] private Sprite _closeSprite;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] private Sprite _lootedSprite;
    private bool isLooted;
    private SpriteRenderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.ActiveRoom.AddTile(this);
    }

    private void Open()
    {
        var rand = Random.Range(1, 101);
        if (rand <= 30)
        {
            //heal
            var max = Player.Instance.MaxHitPoints;
            Player.Instance.Heal((uint) Random.Range( (float) (0.1* max), (float)(0.3*max)));
        }
        // else if(rand <=70)
        // {
        //     //add money
        //     var money =Random.Range(10, 31);
        //     Player.Instance.CollectMoney((uint) money);
        // }
        else
        {
            //add ability
            var unOppened = _abilityList.Except(Player.Instance.Abilities).ToList();
            if ( unOppened.Count == 0)
            {
                Open();
            }
            else
            {
               var ability = unOppened[Random.Range(0, unOppened.Count)];
               Player.Instance.AddAbility(ability);
            }
        }

        isLooted = true;
        _renderer.sprite = _lootedSprite;


    }

    public bool CanStep => GameManager.Instance.ActiveRoom.GetAllEntities().Count == 0;
    public void Step(Transform target)
    {
        if(isLooted) return;
        if(target.TryGetComponent(out Player player))
            Open();
    }

    public void NextTurn()
    {
        if(isLooted) return;
        _renderer.sprite = CanStep ? _openSprite : _closeSprite;
    }

    public Vector2 Position => transform.position;
}
