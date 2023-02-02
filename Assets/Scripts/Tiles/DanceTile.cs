using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Dance{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DanceTile : MonoBehaviour, IFloorTile
{
    [SerializeField] private Color _color;
    [SerializeField] private int _updateTurns;
    [SerializeField] private Sprite[] _sprites;
    private int _turnNow;
    private SpriteRenderer _sprite;
    [SerializeField]private Light2D _light;
    public bool ActiveTile => _turnNow <= 0;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _light = Instantiate(_light, transform);
        RandomColor();
        _sprite.sprite = _sprites[Random.Range(0, _sprites.Length)];
        GameManager.Instance.ActiveRoom.AddTile(this);
    }
    
    [ContextMenu("RandomColor")]
    public void RandomColor()
    {
        var color = (Color) Random.Range(1, 6);
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        _color = color;
        _sprite ??= GetComponent<SpriteRenderer>();
        if(ActiveTile)
            _sprite.color = GameManager.Colors[_color];
        _light.color = GameManager.Colors[_color];
    }
    public void NextTurn()
    {
        _turnNow--;
        _sprite.color = !ActiveTile ? UnityEngine.Color.grey : GameManager.Colors[_color];
        _light.intensity = !ActiveTile ? 0 : 2;
    }

    public Vector2 Position => transform.position;

    public bool CanStep => true;
    public void Step(Transform target)
    {
        if (!ActiveTile) return;
        if(target.TryGetComponent(out IDancer dancer))
        {
            dancer.AddColor(_color);
            _turnNow = _updateTurns+1;
            RandomColor();
        }
    }
}
}