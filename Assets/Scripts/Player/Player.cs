using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using Color = Dance.Color;
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour, IDamageable, IDancer, IEntityObservable
{
    [Serializable]
    public struct Stats
    {
        public int Damage;
        public int Health;
        public int MaxHealth;
        public int Resist;
        
        public static Stats operator+(Stats a, Stats b)
        {
            return new Stats
            {
                Damage = a.Damage + b.Damage,
                Health = a.Health + b.Health,
                MaxHealth = a.MaxHealth + b.MaxHealth,
                Resist = a.Resist + b.Resist
            };
        }
        public static Stats operator-(Stats a, Stats b)
        {
            return new Stats
            {
                Damage = a.Damage - b.Damage,
                Health = a.Health - b.Health,
                MaxHealth = a.MaxHealth - b.MaxHealth,
                Resist = a.Resist - b.Resist
            };
        }
    }
    
    
    public GameInput input;
    private Dance.Color[] _colorSequence = new Dance.Color[8];
    [SerializeReference] private List<Ability> _abilities;
    [SerializeField] private Stats _stats;
    [SerializeField] private Animator _healAnimator;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _abilitySound;
    [SerializeField] private SpriteRenderer _shieldSprite;


    [SerializeField] private Item _testitem;
    
    private AudioSource _audioSource;
    private Animator _animator;
    private PlayerInventory _inventory;
    public PlayerInventory Inventory => _inventory;
    private int _coins;
    public event Action<int> CoinsChanged;
    public event Action<int> HpChanged;

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = _coins + value < 0 ? 0 : value;
            CoinsChanged?.Invoke(_coins);
        }
    }

    public int HitPoints
    {
        get => _stats.Health;
        set
        {
            _stats.Health = value;
            
            if(_stats.Health > _stats.MaxHealth)
                _stats.Health = _stats.MaxHealth;
            if(_stats.Health < 0)
                _stats.Health = 0;
            HpChanged?.Invoke(_stats.Health);
            if (_stats.Health <= 0)
            {
                Die();
            }
        }
    }
    public int MaxHitPoints => _stats.MaxHealth;
    [SerializeField] private Shield _activeShield;
    public IReadOnlyList<Ability> Abilities => _abilities;
    public static Player Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        input = new GameInput();
        input.Enable();
        input.Player.Move.performed += context => Move(context.ReadValue<Vector2>());
        input.Player.Sumbit.performed += context => UseAbility();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _inventory = new PlayerInventory();
        
        if(_testitem != null)
            EquipItem(Instantiate(_testitem));
    }

    private void Move(Vector2 dir)
    {
        if (Math.Abs(dir.x) < 1 && Math.Abs(dir.y) < 1)
            return;
        var entity = GameManager.Instance.ActiveRoom.GetAllEntities().FirstOrDefault(en => (Vector2)en.transform.position == (Vector2)transform.position + dir);
        if (entity != null)
        {
            Attack(entity);
        }
        else if (GameManager.Instance.ActiveRoom.CanMove((Vector2) transform.position + dir, out IFloorTile tile ))
        {
            transform.position += (Vector3)dir;
            tile.Step(transform);
            Moved?.Invoke((int) transform.position.x, (int) transform.position.y);
        }
        else
        {
            return;
        }
        GameManager.Instance.NextTurn();
        if (_activeShield == null) return;
        _activeShield.NextTurn();
        _shieldSprite.enabled = _activeShield.Active;
    }

    private void Attack(Entity entity)
    {
        entity.TakeDamage((uint) _stats.Damage);
        Attacked?.Invoke(entity, _stats.Damage);
        PlaySound(_hitSound);
    }

    public void AddColor(Dance.Color color)
    {
        for (int i = _colorSequence.Length-1; i > 0; i--)
        {
            _colorSequence[i] = _colorSequence[i - 1];
        }

        _colorSequence[0] = color;
        UserInterface.Instance.UpdateSequence(_colorSequence);
    }

    private void UseAbility()
    {
        using var sequences= _abilities.OrderByDescending(ab => ab.Sequence.Count).GetEnumerator();
        bool used = false;
        while (sequences.MoveNext())
        {
            var index = FindSubArrayIndex(sequences.Current.Sequence);
            if (index != -1)
            {
                sequences.Current.Use(this, transform.position);
                used = true;
                for (int i = index; i < sequences.Current.Sequence.Count + index; i++)
                {
                    _colorSequence[i] = Color.Null;
                }
            }
        }
        if(used)
            PlaySound(_abilitySound);
        UserInterface.Instance.UpdateSequence(_colorSequence);
    }

    private int FindSubArrayIndex(IReadOnlyList<Dance.Color> sequence)
    {
        for (int i = 0; i < _colorSequence.Length - sequence.Count + 1; i++)
        {
            var index = i;
            for (int j = 0; j < sequence.Count; j++)
            {
                if (_colorSequence[i + j] != sequence[j])
                {
                    index = -1;
                    break;
                }
            }
            if (index >= 0) 
                return index;
        }
        return -1;
    }

    public void AddAbility(Ability ability)
    {
        _abilities.Add(ability);
    }

    public void Heal(uint heal)
    {
        HitPoints += (int) heal;
        Healed?.Invoke((int) heal);
        _healAnimator.SetTrigger("Heal");
    }

    public void Kill()
    {
        HitPoints = 0;
    }

    public void AddShield(Shield shield)
    {
        _activeShield = shield;
        _shieldSprite.enabled = true;
        _shieldSprite.color = _activeShield.Color;
    }

    public void EquipItem(Item item)
    {
       _stats = _inventory.Equip(item, _stats);
       UserInterface.Instance.UpdateStats();
    }
    
    public void UnequipItem(Item item)
    {
        _stats = _inventory.Unequip(item, _stats);
        UserInterface.Instance.UpdateStats();
    }
    
    public void TakeDamage(uint damage, IDamageable source = null)
    {
        if (_activeShield != null)
            damage = _activeShield.Defence(this,damage);
        damage-= (uint)_stats.Resist;
        TakedDamage?.Invoke(source,(int) damage);
        if (HitPoints > 0)
            HitPoints -= (int)damage;
        _animator.SetTrigger("TakeDamage");
        StartCoroutine(GameManager.Instance.ShakeCamera());
    }

    private void Die()
    {
        Died?.Invoke();
        if(HitPoints <= 0)
            UserInterface.Instance.ShowLosePanel();
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public event Action<IDamageable, int> TakedDamage;
    public event Action<int> Healed;
    public event Action<int, int> Moved;
    public event Action<IDamageable, int> Attacked;
    public event Action Died;
}
