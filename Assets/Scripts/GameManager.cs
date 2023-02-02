using System.Collections;
using System.Collections.Generic;
using Abilities;
using Cinemachine;
using Generators;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Dictionary<Dance.Color, Color> Colors = new()
    {
        {Dance.Color.Yellow, new Color(240/255.0f, 196/255.0f, 25/255.0f) },
        {Dance.Color.Blue, new Color(45/255.0f, 149/255.0f, 191/255.0f)},
        {Dance.Color.Green, new Color(78/255.0f, 186/255.0f, 111/255.0f)},
        {Dance.Color.Red, new Color(241/255.0f, 90/255.0f, 90/255.0f)},
        {Dance.Color.Purple, new Color(149/255.0f, 91/255.0f, 165/255.0f)},
        {Dance.Color.Null, Color.grey}
    };
    
    [Header("Generators")]
    [SerializeField] private DefaultMapGenerator _mapGenerator;
    
    private IMapGenerator _activeGenerator;
    
    [SerializeField]private Room _activeRoom;
    
    [SerializeField] private CinemachineVirtualCamera _camera;
    
    [Header("Items and Spells")]
    [SerializeField] private List<Item> _items;
    [SerializeField] private List<Ability> _spells;
    
    private AudioSource _source;
    public Room ActiveRoom
    {
        get => _activeRoom;
        set
        {
            _activeRoom = value;
        }
    }
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
      
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        
        _activeGenerator = Instantiate(_mapGenerator);

        _activeGenerator.GenerateMap();

        _activeRoom = _activeGenerator.GetStartRoom();
        _source = GetComponent<AudioSource>();
    }
    
    public Item GetRandomItem()
    {
        if(_items.Count == 0) return null;
        var it = _items[Random.Range(0, _items.Count)];
        _items.Remove(it);
        return it;
    }
    public Ability GetRandomSpell()
    {
        if(_spells.Count == 0) return null;
        var it = _spells[Random.Range(0, _spells.Count)];
        _spells.Remove(it);
        return it;
    }
    
    public void NextTurn()
    {
        ActiveRoom.UpdateMap();
    }
    
    private CinemachineBasicMultiChannelPerlin _noise;
    
    public Room.RoomType[,] GetMap()
    {
        return _activeGenerator.GetMap();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void Play(AudioClip clip)
    {
        _source.Stop();
        _source.clip = clip;
        _source.Play();
    }

    public IEnumerator ShakeCamera()
    {
        _noise.m_AmplitudeGain = 2;
        _noise.m_FrequencyGain = 2;
        yield return new WaitForSeconds(.2f);
        _noise.m_AmplitudeGain = 0;
        _noise.m_FrequencyGain = 0;
    }

    public Room GetRoomAtPos(Vector2Int pos)
    {
        return _activeGenerator.GetRoomAtPos(pos);
    }
}
