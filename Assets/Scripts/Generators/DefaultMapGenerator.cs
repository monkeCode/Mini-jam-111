using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generators
{
    [CreateAssetMenu(menuName = "MapGenerator/Default")]
    public class DefaultMapGenerator : ScriptableObject,  IMapGenerator
    {

        [SerializeField] private Room _startRoom;
        [SerializeField] private Room _bossRoom;
        [SerializeField] private Room _shopRoom;
        [SerializeField] private Room[] _rooms;
        
        private Room.RoomType[,] _map;
        private List<Room> _roomList = new();
        
        public virtual Room.RoomType[,] GenerateMap()
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

        var map = new Room.RoomType[10, 10];
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
        _map = map;
        return map;
    }

        public virtual Room GetRoomAtPos(Vector2Int pos)
        {
            var room = _roomList.FirstOrDefault(r => r.Pos == pos);
            if (room != null) return room;
            var type = _map[pos.x, pos.y];
            room = type switch
            {
                Room.RoomType.Boss => GenerateBossRoom(),
                Room.RoomType.Shop => GenerateShopRoom(),
                Room.RoomType.Simple => GenerateRoom(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return InstantiateRoom(room, pos);
        }

        public virtual Room GetStartRoom()
        {

            return InstantiateRoom(_startRoom, new Vector2Int(5,5));
        }

        public Room.RoomType[,] GetMap()
        {
            return (Room.RoomType[,]) _map.Clone();
        }

        private Room InstantiateRoom(Room room, Vector2Int pos)
        {
            var r = Instantiate(room);
            r.SetDoors(pos, new []{_map[pos.x-1,pos.y],_map[pos.x,pos.y-1],_map[pos.x+1,pos.y],_map[pos.x,pos.y+1]});
            _roomList.Add(r);
            return r;
        }
        
        protected virtual Room GenerateRoom()
        {
            var room = _rooms[Random.Range(0, _rooms.Length)];
            return room;
        }
        protected virtual Room GenerateBossRoom()
        {
            return _bossRoom;
        }

        protected virtual Room GenerateShopRoom()
        {
            return _shopRoom;
        }


    
    }
}
