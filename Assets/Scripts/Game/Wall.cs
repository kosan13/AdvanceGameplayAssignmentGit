using UnityEngine;

namespace Game
{
    public sealed class Wall
    {
        public Wall(Vector3 worldPosition) { WorldPosition = worldPosition; }
        public Wall(int indexPositionsX, int indexPositionsY)
        {
            IndexPositionsX = indexPositionsX;
            IndexPositionsY = indexPositionsY;
        }

        public Vector3 WorldPosition { get; set; }
        
        public int IndexPositionsX { get; }
        public int IndexPositionsY { get; }
        public Vector2Int IndexPositions => new(IndexPositionsX, IndexPositionsY);
    }
}