using TileSystem.Tile_Class;
using Unity.Mathematics;
using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        private Tile[,] TileMap  { get; }
        private Vector2Int Size  { get; }
        private float WallHeight { get; }
    }
}