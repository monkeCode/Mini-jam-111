using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Room _shopRoom;
    [SerializeField] private Room[] _rooms;
    [SerializeField] private CinemachineVirtualCamera _camera;
    
    [Header("Items and Spells")]
    [SerializeField] private List<Item> _items;
    [SerializeField] private List<Ability> _spells;
     private Room.RoomType[,] _map;
     private List<Room> _roomList;
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
        
        _map = GenerateMap();

        _roomList = new List<Room>();
        ActiveRoom = Instantiate(_startRoom);
        ActiveRoom.SetDoors(new Vector2Int(5,5),new []{_map[4,5],_map[5,4],_map[6,5],_map[5,6]});
        _roomList.Add(ActiveRoom);
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
    private Room GenerateRoom()
    {
        var room = _rooms[Random.Range(0, _rooms.Length)];
        return Instantiate(room);
    }
    private Room GenerateBossRoom()
    {
        return Instantiate(_bossRoom);
    }

    private Room GenerateShopRoom()
    {
        return Instantiate(_shopRoom);
    }

    public Room GetRoomAtPos(Vector2Int pos)
    {
        var room = _roomList.FirstOrDefault(r => r.Pos == pos);
        if (room != null) return room;
        var type = _map[pos.x, pos.y];
        room = type switch
        {
            Room.RoomType.Boss => GenerateBossRoom(),
            Room.RoomType.Shop => GenerateShopRoom(),
            Room.RoomType.Simple => GenerateRoom(),
        };
        room.SetDoors(pos, new []{_map[pos.x-1,pos.y],_map[pos.x,pos.y-1],_map[pos.x+1,pos.y],_map[pos.x,pos.y+1]});
        _roomList.Add(room);
        return room;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private Room.RoomType[,] GenerateMap()
    {
        int CurrentRooms(int countOfRooms, int allRoomsCount, Room.RoomType[,] roomTypes, int i, int j, ref bool shopRoomEx,
            ref bool bossRoomExist1)
        {
            var rand = Random.Range(0, 101);
            if (rand <= (countOfRooms + 1) / allRoomsCount * 100 && !bossRoomExist1)
            {
                roomTypes[i, j] = Room.RoomType.Boss;
                countOfRooms++;
                bossRoomExist1 = true;
            }
            else if (rand <= countOfRooms / allRoomsCount * 100 && !shopRoomEx)
            {
                roomTypes[i, j] = Room.RoomType.Shop;
                countOfRooms++;
                shopRoomEx = true;
            }
            else if (rand < 30)
            {
                roomTypes[i, j] = Room.RoomType.Simple;
                countOfRooms++;
            }

            return countOfRooms;
        }

        Room.RoomType[,] map = new Room.RoomType[10, 10];
        int roomsCount = Random.Range(5,10);
        int currentRooms = 0;
        bool bossRoomExist = false;
        bool shopRoomExist = false;
        map[5, 5] = Room.RoomType.Simple;
        while (currentRooms < roomsCount)
        {
            for(int i = 0; i < map.GetLength(0) && currentRooms <= roomsCount; i++)
                for(int j = 0; j < map.GetLength(1) && currentRooms <= roomsCount; j++)
                {
                    
                    if (map[i, j] == Room.RoomType.Simple || map[i,j] == Room.RoomType.Shop )
                    {
                        if (i + 1 < map.GetLength(0) && map[i + 1, j] == 0)
                        {
                            currentRooms = CurrentRooms(currentRooms, roomsCount, map, i+1, j, ref shopRoomExist, ref bossRoomExist);
                        }
                        if (i - 1 >= 0 && map[i - 1, j] == 0)
                        {
                            currentRooms = CurrentRooms(currentRooms, roomsCount, map, i-1, j, ref shopRoomExist, ref bossRoomExist);
                        }
                        if (j + 1 < map.GetLength(1) && map[i, j + 1] == 0)
                        {
                            currentRooms = CurrentRooms(currentRooms, roomsCount, map, i, j+1, ref shopRoomExist, ref bossRoomExist);
                        }
                        if (j - 1 >= 0 && map[i, j - 1] == 0)
                        {
                            currentRooms = CurrentRooms(currentRooms, roomsCount, map, i, j-1, ref shopRoomExist, ref bossRoomExist);
                        }
                    }
                }
        }

        return map;
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
