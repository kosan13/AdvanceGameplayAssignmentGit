using System.Collections.Generic;
using TileSystem.Tile_Class;
using UnityEngine;

namespace TileSystem
{
    public static class TileSystemStructs
    {
        public struct NeighborsSearchData
        {
            public readonly Queue<Tile> Open;
            public readonly HashSet<Tile> Closed;
            public (Queue<Tile>, Queue<Tile>) OpenQueue;
            public (HashSet<Tile>, HashSet<Tile>) ClosedHashSet;

            public NeighborsSearchData(Queue<Tile> open, HashSet<Tile> closed, (Queue<Tile>, Queue<Tile>) openQueue, (HashSet<Tile>, HashSet<Tile>) closedHashSet)
            {
                Open = open;
                Closed = closed;
                OpenQueue = openQueue;
                ClosedHashSet = closedHashSet;
            }
        }
        public struct RecursiveFloodFillData
        {
            public (HashSet<Tile>, HashSet<Tile>) RecursiveItems { get; }
            public int MinRoomSize { get; }

            public RecursiveFloodFillData((HashSet<Tile>, HashSet<Tile>) recursiveItems, int minRoomSize = 5)
            {
                RecursiveItems = recursiveItems;
                MinRoomSize = minRoomSize;
            }
        }

        public struct Wall
        {
            public Mesh Mesh;
            public (Tile, Tile) WallsOwners;

            public Wall(Mesh mesh, (Tile, Tile) wallsOwners)
            {
                Mesh = mesh;
                WallsOwners = wallsOwners;
            }
        }
    }
}