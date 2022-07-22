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
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private Ways _ways;
    [Serializable]
    private struct Ways
    {
        public Way Left;
        public Way Right;
        public Way Top;
        public Way Bottom;
    }
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
    public Vector2 MoveToPos(Vector2 start, Vector2 end)
    {
        return Vector2.zero;
    }
}
