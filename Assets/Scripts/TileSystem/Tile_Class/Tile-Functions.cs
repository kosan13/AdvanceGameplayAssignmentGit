using System.Collections.Generic;
using Graphs;
using UnityEngine;

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
    }
}