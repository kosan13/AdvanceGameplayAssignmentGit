using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace TileSystem
{
    
    [BurstCompile]
    public struct NativeTileMap
    {
        public NativeTileMap(NativeArray<NativeArrayContainer> tileMap, int2 size, float wallHeight)
        {
            TileMap = tileMap;
            Size = size;
            WallHeight = wallHeight;
        }

        private NativeArray<NativeArrayContainer> TileMap  { get; }
        private int2 Size  { get; }
        private float WallHeight { get; }
        
        #region Properties
        public NativeArray<NativeArrayContainer> GetTileMap => TileMap;

        public int2 GetSize => Size;
        public int GetSizeX => Size.x;
        public int GetSizeY => Size.y;
        
        public float GetWallHeight => WallHeight;
        
        public int GetLength => TileMap.Length;
        public bool IsEmpty => TileMap.Length <= 0;
        
        #endregion
        
        public NativeTile GetTile(int indexX, int indexY) => IsValidIndex(indexX, indexY) ? TileMap[indexX].Tiles[indexY] : new NativeTile();
        public NativeTile GetTile(int2 index) => GetTile(index.x, index.y);
        public bool IsValidIndex(int indexX, int indexY) => indexX < GetSizeX && indexY < GetSizeY && indexX >= 0 && indexY >= 0;
        public bool IsValidIndex(int2 index) => IsValidIndex(index.x, index.y);
    }
}