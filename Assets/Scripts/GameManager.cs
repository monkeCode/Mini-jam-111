using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<Dance.Color, Color> Colors = new()
    {
        {Dance.Color.Yellow, new Color(240/255.0f, 196/255.0f, 25/255.0f) },
        {Dance.Color.Blue, new Color(45/255.0f, 149/255.0f, 191/255.0f)},
        {Dance.Color.Green, new Color(78/255.0f, 186/255.0f, 111/255.0f)},
        {Dance.Color.Red, new Color(241/255.0f, 90/255.0f, 90/255.0f)},
        {Dance.Color.Purple, new Color(149/255.0f, 91/255.0f, 165/255.0f)}
    };
    
    [SerializeField]private Room _activeRoom;

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
    }
    
    void Update()
    {
        
    }

    public void NextTurn()
    {
        ActiveRoom.UpdateMap();
    }
}
