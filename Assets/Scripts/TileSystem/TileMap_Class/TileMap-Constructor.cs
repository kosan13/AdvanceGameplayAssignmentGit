using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region Constructor
        
        public TileMapClass(Vector2Int size, float wallHeight = 0) 
        { 
            Size = size;
            TileMap = new Tile_Class.Tile[size.x, size.y];
            WallHeight = wallHeight;
        }
        public TileMapClass(int indexPositionX, int indexPositionY, float wallHeight = 0) 
        { 
            Size = new Vector2Int(indexPositionX, indexPositionY);
            TileMap = new Tile_Class.Tile[indexPositionX, indexPositionY];
            WallHeight = wallHeight;
        }
        
        #endregion
    }
}