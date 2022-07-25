using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [SerializeField] private Room _startRoom;
    [SerializeField]private Room _activeRoom;
    [SerializeField] private Room _bossRoom;
    [SerializeField] private Room[] _rooms;
    [SerializeField] private CinemachineVirtualCamera _camera;
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
        ActiveRoom = Instantiate(_startRoom);
        _source = GetComponent<AudioSource>();
    }
    

    public void NextTurn()
    {
        ActiveRoom.UpdateMap();
    }

    public bool bossRoomExist = false;
    private int countOfRooms = 1;
    private CinemachineBasicMultiChannelPerlin _noise;
    public bool CanGenerateBossRoom => !bossRoomExist && countOfRooms >= 5;
    public Room GenerateRoom()
    {
        var room = _rooms[Random.Range(0, _rooms.Length)];
        countOfRooms++;
        return Instantiate(room);
    }
    public Room GenerateBossRoom()
    {
        bossRoomExist = true;
        countOfRooms++;
        return Instantiate(_bossRoom);
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
}
