using Librarys.Graphs.Scripts;
using Unity.Collections;
using Unity.Mathematics;

namespace TileSystem.Tile_Class
{
    public partial class Tile
    {

        public struct TileMultithreadingValus
        {
            private float3 WorldPosition { get; set; }
            private int2 IndexPositions { get; set; }
            // private NativeList<Link> Links { get; set; }
            private int RegionID { get; set; }
            private bool IsOccupied { get; set; }
            private bool CanBeScanned { get; set; }
        }
    }
}