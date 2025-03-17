using System.Collections.Generic;
using System.Linq;
using TileSystem.Tile_Class;

namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region static
        
        public static bool IsScannableTile(Tile tile) => tile.GetCanBeScanned && !tile.GetIsOccupied;
        public static HashSet<Tile>[] AllRegion(TileMapClass tileMap ,int regionID)
        {
            HashSet<Tile>[] allRegion = new HashSet<Tile>[regionID];
            for (int index = 0; index < allRegion.Length; index++) allRegion[index] = new HashSet<Tile>();
            foreach (Tile tile in tileMap.GetTileMap) allRegion[tile.GetRegionID].Add(tile);
            
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
        
        #endregion
    }
}