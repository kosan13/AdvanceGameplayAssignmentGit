namespace TileSystem.TileMap_Class
{
    public sealed partial class TileMapClass
    {
        #region static
        
        public static bool IsScannableTile(Tile_Class.Tile tile) => tile.GetCanBeScanned && !tile.GetIsOccupied;
        
        #endregion
    }
}