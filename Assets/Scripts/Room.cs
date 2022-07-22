using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dance;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField] private Tilemap _groundTileMap;
    private List<ITurned> _entities;
    [SerializeField] private Ways _ways;
    [Serializable]
    private struct Ways
    {
        public Way Left;
        public Way Right;
        public Way Top;
        public Way Bottom;

        public void Open()
        {
            Left.OpenDoor();
            Right.OpenDoor();
            Top.OpenDoor();
            Bottom.OpenDoor();
        }

        public void Close()
        {
            Left.CloseDoor();
            Right.CloseDoor();
            Top.CloseDoor();
            Bottom.CloseDoor();
        }
    }

    private List<DanceTile> _tiles = new List<DanceTile>();
    void Start()
    {
    }

    public bool CanMove(Vector2 pos)
    {
       var vec =  _groundTileMap.WorldToCell(pos);
       return _groundTileMap.HasTile(vec);
    }

    public DanceTile GetFloorTile(Vector2 pos)
    {
        var vec =  _groundTileMap.WorldToCell(pos);
        var item = _groundTileMap.gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(obj => obj.position == vec);
        if (item != null)
        {
            if (item.TryGetComponent(out DanceTile danceTile))
            {
                return danceTile;
            }
        }
        return null;
    }
    public Stack<Vector2> MoveToPos(Vector2 start, Vector2 end)
    {
        return null;
    }

    public void UpdateMap()
    {
        _tiles.ForEach(it => it.NextTurn());
        _entities.ForEach(it => it.NextTurn());
        if(_entities.Count > 0)
            _ways.Close();
        else
        {
            _ways.Open();
        }
    }

    public void AddEntity(ITurned entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(ITurned entity)
    {
        _entities.Remove(entity);
    }
    public void AddTile(DanceTile tile)
    {
        _tiles.Add(tile);
    }
}
