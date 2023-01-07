using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dance;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        NoRoom,
        Simple,
        Boss,
        Shop
    }
    private List<ITurned> _entities = new();
    [SerializeField] private Ways _ways;
    [Serializable]
    private class Ways
    {
        public Way Left;
        public Way Right;
        public Way Top;
        public Way Bottom;

        public IReadOnlyList<Way> GetWays()
        {
            return new List<Way> {Left, Top, Right, Bottom};
        }
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

    private List<IFloorTile> _tiles = new();
    public IReadOnlyList<IFloorTile> Tiles => _tiles;
    private IFloorTile[,] _logicalMap;
    private Vector2Int _offset;
    private int? _bossWay = null;
    private int? shopWay = null;
    public Vector2Int Pos;
    private void Start()
    {
        _ways.Close();
    }

    public void SetDoors(Vector2Int pos,RoomType[] doors)
    {
        Pos = pos;
        for(int i = 0; i < doors.Length; i++)
        {
            if(doors[i] ==RoomType.NoRoom)
                _ways.GetWays()[i].BlockDoor();
            if (doors[i] == RoomType.Boss)
            {
                _bossWay = i;
                _ways.GetWays()[i].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (doors[i] == RoomType.Shop)
            {
                shopWay = i;
            }
        }
    }
    
    private void UpdateLogicalMap()
    {
        var minVector = new Vector2Int((int) _tiles.Min(tile => tile.Position.x), 
            (int) _tiles.Min(tile=> tile.Position.y));
        var maxVector = new Vector2(_tiles.Max(tile => tile.Position.x),
            _tiles.Max(tile => tile.Position.y));
        var matrix = new IFloorTile[(int) (maxVector.y - minVector.y)+1,(int) (maxVector.x - minVector.x)+1];
        foreach (var tile in _tiles)
        {
            var pos = tile.Position;
            matrix[(int) (pos.y - minVector.y), (int) (pos.x - minVector.x)] = tile;
        }

        for(int i = 0; i < matrix.GetLength(0);i++)
        {
            string s = "";
            for (int j = 0; j < matrix.GetLength(1); j++)
                s += matrix[i, j] != null? "1 ":"0 ";
        }

        _offset = minVector;
        _logicalMap = matrix;
    }
    public bool CanMove(Vector2 pos, out IFloorTile tile)
    {
        UpdateLogicalMap();
        tile = GetFloorTile(pos); 
        return tile is {CanStep: true};
    }

    public IFloorTile GetFloorTile(Vector2 pos)
    {
        try
        {
            var item = _logicalMap[(int) (pos.y - _offset.y), (int) (pos.x - _offset.x)];
            return item;
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }
    public IFloorTile GetFloorTile(int x, int y)
    {
        return GetFloorTile(new Vector2(x, y));
    }
    public Stack<Vector2Int> MoveToPos(Vector2 start, Vector2 end)
    {
        if (start == end)
            return null;
        const int countOfRepeats = 100;
        var matrix = new int[_logicalMap.GetLength(0), _logicalMap.GetLength(1)];
        start -= _offset;
        end -= _offset;
        for(int i = 0; i < _logicalMap.GetLength(0); i++)
        for(int j = 0; j < _logicalMap.GetLength(1); j++)
        {
            matrix[i,j] = (_logicalMap[i,j] != null && _logicalMap[i,j].CanStep)?int.MaxValue : -1;
        }

        foreach (var ent in _entities)
        {
            var pos = ((Entity)ent).transform.position;
            matrix[(int) (pos.y - _offset.y), (int) (pos.x - _offset.x)] = -1;
        }
        matrix[(int) start.y,(int) start.x] = 0;

        int[] Neighbors(int i1, int i2)
        {
            int[] neighbors1 = {-1, -1, -1, -1};
            if (i1 - 1 >= 0)
                neighbors1[0] = matrix[i2, i1 - 1];
            if (i1 + 1 < matrix.GetLength(1))
                neighbors1[1] = matrix[i2, i1 + 1];
            if (i2 - 1 >= 0)
                neighbors1[2] = matrix[i2 - 1, i1];
            if (i2 + 1 < matrix.GetLength(0))
                neighbors1[3] = matrix[i2 + 1, i1];
            return neighbors1;
        }
        //mark array
        for (int i = 0; i < countOfRepeats; i++)
        {
            if(matrix[(int) end.y,(int) end.x] != int.MaxValue) break;
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    if(matrix[y,x] == -1) continue;
                    var neighbors = Neighbors(x, y);
                    try
                    {
                        var min = neighbors.Where(item => item >= 0).Min();
                        if (min != int.MaxValue)
                            min++;
                        if(min < matrix[y, x])
                            matrix[y, x] = min;
                    }
                    catch (Exception _)
                    {
                        // ignored
                    }
                }
            }
        }

        if (matrix[(int) end.y, (int) end.x] == int.MaxValue || matrix[(int) end.y, (int) end.x] == -1 )
            return null;
        var points = new Stack<Vector2Int>();
        var currentPlace = new Vector2Int((int) end.x,(int) end.y);
        var intStart = new Vector2Int((int) start.x, (int) start.y);
        while (currentPlace != intStart )
        {
            var neighbors = Neighbors(currentPlace.x, currentPlace.y);
            var min = neighbors.Where(item => item >= 0).Min();
            var index = Array.FindIndex(neighbors, it => it == min);
            var vector2Int = index switch
            {
                0 => Vector2Int.right,
                1 => Vector2Int.left,
                2 => Vector2Int.up,
                3 => Vector2Int.down,
                _ => Vector2Int.zero
            };
            Debug.DrawRay((Vector2)currentPlace + _offset, (Vector2)vector2Int/2, Color.cyan, 1);
            points.Push(vector2Int);
            currentPlace -= vector2Int;
        }

        return points;
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

    public void AddTile(IFloorTile tile)
    {
        _tiles.Add(tile);
    }

    public IReadOnlyList<Entity> GetAllEntities()
    {
        return _entities.Select(en => (Entity) en).ToList();
    }

    public void Exit(Way way)
    {
        int index = (int) way.Dir;
        Room room = null;
        if (index == 0)
        {
            room = GameManager.Instance.GetRoomAtPos(Pos+Vector2Int.left);
        } 
        if (index == 1)
        {
            room = GameManager.Instance.GetRoomAtPos(Pos-Vector2Int.up);
        }
        if (index == 2)
        {
            room = GameManager.Instance.GetRoomAtPos(Pos+Vector2Int.right);
        }
        if (index == 3)
        {
            room = GameManager.Instance.GetRoomAtPos(Pos-Vector2Int.down);
        }
        
        Vector2 pos = Vector2.zero;
        pos = room._ways.GetWays()[(index + 2) % 4].EnterPonit;
        
        Player.Instance.transform.position = pos;
        GameManager.Instance.ActiveRoom = room;
        room.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    public Vector2 RandomValidPosition()
    {
            var pos = Vector2.zero;
        do
        {
            pos = new Vector2(Random.Range(0, _logicalMap.GetLength(1)), Random.Range(0, _logicalMap.GetLength(0)));
            pos += _offset;
        } while (GetFloorTile(pos) as DanceTile == null || 
                 GetAllEntities().Count(entity => (Vector2)entity.transform.position == pos) != 0 ||
                 pos == (Vector2)Player.Instance.transform.position);
        return pos;
    }
}
