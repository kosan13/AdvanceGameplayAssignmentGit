namespace TileSystem.Tile_Class
{
    public partial class Tile
    {
        #region static
            
        public static bool IsSameRegionID(Tile tileOne, Tile tileTwo) => tileOne.RegionID == tileTwo.RegionID;
            
        #endregion
    }
}