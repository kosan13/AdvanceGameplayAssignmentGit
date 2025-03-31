using System.Collections.Generic;
using Librarys.Graphs.Interfaces;
using Librarys.Graphs.Scripts;
using UnityEngine;

namespace TileSystem.Tile_Class
{
    public partial class Tile : IPositionNode
    {
        private Vector3 WorldPosition { get; set; }
        private Vector2Int IndexPositions { get; set; }
        private List<Link> Links { get; set; }
        private int RegionID { get; set; }
        private bool IsOccupied { get; set; }
        private bool CanBeScanned { get; set; }
        private Transform Visual { get; set; }
        private Renderer Renderer { get; set; }
        
        private bool VoidArea { get; set; }
    }
}