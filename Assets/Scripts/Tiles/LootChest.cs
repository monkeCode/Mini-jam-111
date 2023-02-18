using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(SpriteRenderer))]
public class LootChest : MonoBehaviour, IFloorTile
{
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
            UserInterface.Instance.CreateText(lootText,transform.position);
            Player.Instance.Coins += countCoins;
            _moneyParticle.Play();
        }
        else
        {
            //add ability
            var spell = GameManager.Instance.GetRandomSpell();
            if ( spell is null)
            { 
                Open();
                return;
            }
            Player.Instance.AddAbility(Instantiate(spell));
            
            UserInterface.Instance.CreateText("New Ability: " + spell.Name, transform.position);

        }
        GameManager.Instance.Play(_openSound);
        isLooted = true;
        _renderer.sprite = _lootedSprite;


    }

    public bool CanStep => GameManager.Instance.ActiveRoom.GetAllEntities().Count == 0;
    public uint StepCost => 1;

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
