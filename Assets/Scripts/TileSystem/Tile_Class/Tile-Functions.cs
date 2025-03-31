using System;
using System.Collections.Generic;
using Librarys.Graphs.Enum;
using Librarys.Graphs.Scripts;
using TileSystem.TileMap_Class;
using UnityEngine;
using static Librarys.Graphs.Enum.Direction;

namespace TileSystem.Tile_Class
{
    public partial class Tile
    {
        public List<Link> SetLinksList(List<Link> linksList) => Links = linksList;
        public List<Link> SetLinksListToNew() => Links = new List<Link>();
        
        public void AddLink(Link link) => Links.Add(link);
        public void RemoveLink(Link link) => Links.Remove(link);
        
        public int SetRegionID(int regionID) => RegionID = regionID;
        public bool SetIsOccupied(bool occupied) => IsOccupied = occupied; 
        public bool SetCanBeScanned(bool scanned) => CanBeScanned = scanned; 
        
        public void SetMaterial(Material material) => Renderer.material = material;
        
        public bool IsSameTile(Tile tile) => this == tile;
        public bool IsSameRegionID(Tile tile) => tile.RegionID == RegionID;
    
        public bool IsNeighbors(Tile tile, bool checkCorners = true)
        {
            if (GetIndexPositionsY == tile.GetIndexPositionsX + 1) return true;
            if (GetIndexPositionsY == tile.GetIndexPositionsX - 1) return true;
            if (GetIndexPositionsX == tile.GetIndexPositionsY + 1) return true;
            if (GetIndexPositionsX == tile.GetIndexPositionsY - 1) return true;

            if (!checkCorners) return false;
            
            if (GetIndexPositionsX - 1 == tile.GetIndexPositionsY - 1) return true;
            if (GetIndexPositionsY + 1 == tile.GetIndexPositionsX + 1) return true;
            if (GetIndexPositionsX - 1 == tile.GetIndexPositionsY + 1) return true;
            if (GetIndexPositionsY - 1 == tile.GetIndexPositionsX + 1) return true;
            return false;
        }
        public Tile GetNeighbour(TileMapClass tilemap, Direction direction)
        {
            Vector2Int neighbourJaggedIndex = direction switch
            {
                Null or Up or Down => IndexPositions,
                Forward or North => IndexPositions + new Vector2Int(-1, 0),
                Back or South => IndexPositions + new Vector2Int(1, 0),
                Right or East => IndexPositions + new Vector2Int(0, 1),
                Left or West => IndexPositions + new Vector2Int(0, -1),
                Northeast => IndexPositions + new Vector2Int(1, 1),
                Southeast => IndexPositions + new Vector2Int(-1, 1),
                Northwest => IndexPositions + new Vector2Int(1, -1),
                Southwest => IndexPositions + new Vector2Int(-1, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (neighbourJaggedIndex.x >= tilemap.GetLength || neighbourJaggedIndex.y >= tilemap.GetLength) return this;
            if (neighbourJaggedIndex.x < 0 || neighbourJaggedIndex.y < 0) return this;

            Tile tile = tilemap.GetTile(neighbourJaggedIndex);
            return tile.VoidArea ? this : tile;
        }
        public Vector2Int GetNeighbourIndex(TileMapClass tilemap, Direction direction) => GetNeighbour(tilemap, direction).GetIndexPositions;
    }
}