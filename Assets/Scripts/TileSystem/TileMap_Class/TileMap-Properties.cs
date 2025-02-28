using Newtonsoft.Json;
using TileSystem.Tile_Class;
using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region Properties
        [JsonIgnore] public Tile[,] GetTileMap => TileMap;

        [JsonIgnore] public Vector2Int GetSize => Size;
        [JsonIgnore] public int GetSizeX => Size.x;
        [JsonIgnore] public int GetSizeY => Size.y;
        
        [JsonIgnore] public float GetWallHeight => WallHeight;
        
        [JsonIgnore] public int GetLength => TileMap.Length;
        [JsonIgnore] public bool IsEmpty => TileMap.Length <= 0;
        
        #endregion
    }
}