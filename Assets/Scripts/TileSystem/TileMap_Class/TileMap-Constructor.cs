using TileSystem.Tile_Class;
using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region Constructor
        
        public TileMapClass(Vector2Int size, float wallHeight = 0) 
        { 
            Size = size;
            TileMap = new Tile[size.x, size.y];
            WallHeight = wallHeight;
        }
        public TileMapClass(int indexPositionX, int indexPositionY, float wallHeight = 0) 
        { 
            Size = new Vector2Int(indexPositionX, indexPositionY);
            TileMap = new Tile[indexPositionX, indexPositionY];
            WallHeight = wallHeight;
        }
        public TileMapClass(Vector2Int size, float wallHeight = 0, bool generateTiles = false) 
        { 
            Size = size;
            TileMap = new Tile[size.x, size.y];
            WallHeight = wallHeight;

            if (!generateTiles) return;
            // Create Tiles
            for (int x = 0; x < GetSizeX; x++)
                for (int y = 0; y < GetSizeY; y++)
                    SetTile(x, y, new Tile(x, y));
        }
        public TileMapClass(int indexPositionX, int indexPositionY, float wallHeight = 0, bool generateTiles = false) 
        { 
            Size = new Vector2Int(indexPositionX, indexPositionY);
            TileMap = new Tile[indexPositionX, indexPositionY];
            WallHeight = wallHeight;

            if (!generateTiles) return;
            // Create Tiles
            for (int x = 0; x < GetSizeX; x++)
                for (int y = 0; y < GetSizeY; y++)
                    SetTile(x, y, new Tile(x, y));
        }
        
        #endregion
    }
}