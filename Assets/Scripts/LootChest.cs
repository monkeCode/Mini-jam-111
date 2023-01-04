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
    [SerializeField] private EventText _eventText;
    [SerializeField] private ParticleSystem _moneyParticle;
    [Header("Sprites")]
    [SerializeField] private Sprite _closeSprite;
    [SerializeField] private Sprite _openSprite;
    [Header("Sounds")]
    [SerializeField] private Sprite _lootedSprite;
    [SerializeField] private AudioClip _openSound;
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
        if (rand <= 60)
        {
            string lootText = "";
            int countCoins = 0;
            //heal
            if (Player.Instance.HitPoints < Player.Instance.MaxHitPoints)
            {

                var max = Player.Instance.MaxHitPoints;
                Player.Instance.Heal((uint) Random.Range((float) (0.1 * max), (float) (0.3 * max)));
                lootText += "Healing\n";
            }
            else
            {
                countCoins += Random.Range(5, 15);
            }

            countCoins += Random.Range(5, 15);
            lootText += $"{countCoins} coins";
            Instantiate(_eventText, transform.position, Quaternion.identity).Init(lootText);
            Player.Instance.Coins += countCoins;
            _moneyParticle.Play();
        }
        else
        {
            //add ability
            var unOpened = _abilityList.Except(Player.Instance.Abilities).ToList();
            if ( unOpened.Count == 0)
            { 
                Open();
                return;
            }
            var ability = unOpened[Random.Range(0, unOpened.Count)];
            Player.Instance.AddAbility(Instantiate(ability));
            Instantiate(_eventText, transform.position, Quaternion.identity).Init("New Ability: " + ability.Name);
               
        }
        GameManager.Instance.Play(_openSound);
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
