using System.Collections;
using System.Collections.Generic;
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

    public Room ActiveRoom
    {
        get => _activeRoom;
        set
        {
            _activeRoom = value;
        }
    }
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        ActiveRoom = Instantiate(_startRoom);
    }
    
    void Update()
    {
        
    }

    public void NextTurn()
    {
        ActiveRoom.UpdateMap();
    }

    private bool _bossRoomExist = false;
    private int countOfRooms = 1;

    public Room GenerateRoom()
    {
        Room room;
        if (_bossRoomExist || countOfRooms <= 4)
        {
            room = _rooms[Random.Range(0, _rooms.Length)];
        }
        else
        {
            room = Random.Range(1, 101) switch
            {
                >= 70 => _bossRoom,
                _ => _rooms[Random.Range(0, _rooms.Length)]
            };
            if (room == _bossRoom)
                _bossRoomExist = true;
        }

        countOfRooms++;
        return Instantiate(room);
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
}
