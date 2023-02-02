using UnityEngine;

namespace Generators
{
   public interface IMapGenerator
   {
      public Room.RoomType[,] GenerateMap();
      public Room GetRoomAtPos(Vector2Int pos);

      public Room GetStartRoom();
      
      public Room.RoomType[,] GetMap();

   }
}
