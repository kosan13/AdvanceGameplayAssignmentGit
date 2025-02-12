using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        public List<Tile_Class.Tile> GetAllTiles()
        {
            // Collect all tiles in the map into a single region
            List<Tile_Class.Tile> allTiles = new ();
            for (int x = 0; x < GetSizeX; x++)
                for (int y = 0; y < GetSizeY; y++)
                    allTiles.Add(GetTile(x, y));
            return allTiles;
        }
        
        public Tile_Class.Tile GetTile(int indexX, int indexY) => IsValidIndex(indexX, indexY) ? TileMap[indexX, indexY] : null;
        public Tile_Class.Tile GetTile(Vector2Int index) => GetTile(index.x, index.y);
        
        public Tile_Class.Tile SetTile(int indexX, int indexY, Tile_Class.Tile newTile) =>  GetTileMap[indexX, indexY] = newTile;
        public Tile_Class.Tile SetTile(Vector2Int index, Tile_Class.Tile newTile) => SetTile(index.x, index.y, newTile);
        
        public bool IsValidIndex(int indexX, int indexY) => indexX < GetSizeX && indexY < GetSizeY && indexX >= 0 && indexY >= 0;
        public bool IsValidIndex(Vector2Int index) => IsValidIndex(index.x, index.y);
        
        public (bool, List<Tile_Class.Tile>) HasNeighborsWideDifferentIds(Tile_Class.Tile tile, bool includeCorners = true)
        {
            Tile_Class.Tile[] neighbors = includeCorners ? new Tile_Class.Tile[8] : new Tile_Class.Tile[4];

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

            List<Tile_Class.Tile> returnNeighbors = neighbors.Where(neighborTile => neighborTile is not null && !tile.IsSameRegionID(neighborTile)).ToList();
            return returnNeighbors.Count <= 0 ? (true, returnNeighbors) : (false, returnNeighbors);
        } 
    }
}