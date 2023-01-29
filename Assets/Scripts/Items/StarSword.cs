using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/StarSword")]
public class StarSword : Item
{
    [SerializeField] private GameObject _star;
    [SerializeField] private int _damage;
    [SerializeField] private int _starCount;
    [SerializeField] private float _chance;
    public override Player.Stats Equip()
    {
        Player.Instance.Moved += Action;
        return base.Equip();
    }

    public override Player.Stats Unequip()
    {
        Player.Instance.Moved -= Action;
        return base.Unequip();
    }

    private void Action(int arg1, int arg2)
    {
        if(Random.Range(0,100) > _chance) return;
        
        var entities = GameManager.Instance.ActiveRoom.GetAllEntities();
        if(entities.Count == 0) return;
        
        for (int i = 0; i < _starCount;  i++)
        {
            var star = Instantiate(_star);
            var position = entities[Random.Range(0, entities.Count)].transform;
            star.GetComponent<Star>().Init(position, _damage);
        }
    }
}
