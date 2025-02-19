using System.Collections.Generic;
using System.Linq;
using TileSystem.Tile_Class;
using Unity.VisualScripting;
using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        public List<Tile> GetAllTiles()
        {
            // Collect all tiles in the map into a single region
            List<Tile> allTiles = new ();
            for (int x = 0; x < GetSizeX; x++)
                for (int y = 0; y < GetSizeY; y++)
                    allTiles.Add(GetTile(x, y));
            return allTiles;
        }
        
        public Tile GetTile(int indexX, int indexY) => IsValidIndex(indexX, indexY) ? TileMap[indexX, indexY] : null;
        public Tile GetTile(Vector2Int index) => GetTile(index.x, index.y);
        
        public Tile SetTile(int indexX, int indexY, Tile newTile) =>  GetTileMap[indexX, indexY] = newTile;
        public Tile SetTile(Vector2Int index, Tile newTile) => SetTile(index.x, index.y, newTile);
        
        public bool IsValidIndex(int indexX, int indexY) => indexX < GetSizeX && indexY < GetSizeY && indexX >= 0 && indexY >= 0;
        public bool IsValidIndex(Vector2Int index) => IsValidIndex(index.x, index.y);
        public (bool, List<Tile>) HasNeighborsWideDifferentIds(Tile tile, bool includeCorners = true)
        {
            Tile[] neighbors = includeCorners ? new Tile[8] : new Tile[4];

            neighbors[0] = GetTile(tile.GetIndexPositionsX + 1, tile.GetIndexPositionsY);
            neighbors[1] = GetTile(tile.GetIndexPositionsX - 1, tile.GetIndexPositionsY);
            neighbors[2] = GetTile(tile.GetIndexPositionsX, tile.GetIndexPositionsY + 1);
            neighbors[3] = GetTile(tile.GetIndexPositionsX, tile.GetIndexPositionsY - 1);
            if (includeCorners)
            {
                neighbors[0] = GetTile(tile.GetIndexPositionsX + 1, tile.GetIndexPositionsY + 1);
                neighbors[1] = GetTile(tile.GetIndexPositionsX - 1, tile.GetIndexPositionsY + 1);
                neighbors[2] = GetTile(tile.GetIndexPositionsX + 1, tile.GetIndexPositionsY - 1);
                neighbors[3] = GetTile(tile.GetIndexPositionsX - 1, tile.GetIndexPositionsY - 1);
            }

            List<Tile> returnNeighbors = neighbors.Where(neighborTile => neighborTile is not null && !tile.IsSameRegionID(neighborTile)).ToList();
            return returnNeighbors.Count <= 0 ? (true, returnNeighbors) : (false, returnNeighbors);
        } 
        public HashSet<Tile>[] AllRegion(int regionID)
        {
            HashSet<Tile>[] allRegion = new HashSet<Tile>[regionID];
            for (int index = 0; index < allRegion.Length; index++) allRegion[index] = new HashSet<Tile>();
            foreach (Tile tile in GetTileMap) allRegion[tile.GetRegionID].Add(tile);
            
            int newLength = allRegion.Count(region => region.Count > 0);
            HashSet<Tile>[] newRegion = new HashSet<Tile>[newLength];
            (int i, int j) = (0, 0);
            while (i < allRegion.Length)
            {
                if (allRegion[i].Count > 0)
                {
                    newRegion[j] = allRegion[i]; 
                    j++;
                } 
                i++;
            }
            return newRegion;
        }
    }
}