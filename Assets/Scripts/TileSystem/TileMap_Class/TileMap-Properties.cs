using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region Properties
        public Tile_Class.Tile[,] GetTileMap => TileMap;

        public Vector2Int GetSize => Size;
        public int GetSizeX => Size.x;
        public int GetSizeY => Size.y;
        
        public float GetWallHeight => WallHeight;
        
        public int GetLength => TileMap.Length;
        public bool IsEmpty => TileMap.Length <= 0;
        
        #endregion
    }
}