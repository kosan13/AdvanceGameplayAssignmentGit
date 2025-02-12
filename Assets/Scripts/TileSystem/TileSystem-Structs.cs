using System.Collections.Generic;

namespace TileSystem
{
    public class TileSystemStructs
    {
        public struct NeighborsSearchData
        {
            public readonly Queue<Tile_Class.Tile> Open;
            public readonly HashSet<Tile_Class.Tile> Closed;
            public (Queue<Tile_Class.Tile>, Queue<Tile_Class.Tile>) OpenQueue;
            public (HashSet<Tile_Class.Tile>, HashSet<Tile_Class.Tile>) ClosedHashSet;

            public NeighborsSearchData(Queue<Tile_Class.Tile> open, HashSet<Tile_Class.Tile> closed, (Queue<Tile_Class.Tile>, Queue<Tile_Class.Tile>) openQueue, (HashSet<Tile_Class.Tile>, HashSet<Tile_Class.Tile>) closedHashSet)
            {
                Open = open;
                Closed = closed;
                OpenQueue = openQueue;
                ClosedHashSet = closedHashSet;
            }
        }
        public struct RecursiveFloodFillData
        {
            public (HashSet<Tile_Class.Tile>, HashSet<Tile_Class.Tile>) RecursiveItems { get; }
            public int MinRoomSize { get; }

            public RecursiveFloodFillData((HashSet<Tile_Class.Tile>, HashSet<Tile_Class.Tile>) recursiveItems, int minRoomSize = 5)
            {
                RecursiveItems = recursiveItems;
                MinRoomSize = minRoomSize;
            }
        }
    }
}