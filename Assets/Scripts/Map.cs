using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private GameObject _coridorPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _layout;

    private List<GameObject> _items = new();

    private void Open()
    {
        _layout.SetActive(true);
        var map = GameManager.Instance.GetMap();
        
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (i > 0 && map[i, j] != Room.RoomType.NoRoom && map[i - 1, j] != Room.RoomType.NoRoom)
                {
                    var cor = Instantiate(_coridorPrefab, _layout.transform);
                    cor.transform.localPosition = new Vector3(i * 45 - 15-250, (map.GetLength(1)- j) * 45-250, 0);
                    _items.Add(cor);
                }

                if (j > 0 && map[i, j] != Room.RoomType.NoRoom && map[i, j - 1] != Room.RoomType.NoRoom)
                {
                    var cor = Instantiate(_coridorPrefab, _layout.transform);
                    cor.transform.localPosition = new Vector3(i * 45-250, (map.GetLength(1)- j) * 45 + 15-250, 0);
                    cor.transform.Rotate(0, 0, 90);
                    _items.Add(cor);
                }
            }

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == Room.RoomType.NoRoom) continue;
                var room = Instantiate(_roomPrefab, _layout.transform);
                room.transform.localPosition = new Vector3(i * 45-250, (map.GetLength(1)- j) * 45-250, 0);
                if(map[i,j] == Room.RoomType.Boss)
                    room.GetComponent<Image>().color = Color.red;
                if(map[i,j] == Room.RoomType.Shop)
                    room.GetComponent<Image>().color = Color.yellow;
                _items.Add(room);
            }
        }
    }

    private void Close()
    {
        foreach (var it in _items)
        {
            Destroy(it);
        }
        _items.Clear();
        _layout.SetActive(false);
    }

    private void OnEnable()
    {
        Open();
    }

    private void OnDisable()
    {
        Close();
    }
}
