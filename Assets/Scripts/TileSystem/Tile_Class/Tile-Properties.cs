using System.Collections.Generic;
using Graphs;
using Newtonsoft.Json;
using UnityEngine;

namespace TileSystem.Tile_Class
{
    public partial class Tile
    {
        #region Properties

        [JsonIgnore] public Vector3 GetWorldPosition => WorldPosition;
        
        [JsonIgnore] public Vector2Int GetIndexPositions => IndexPositions;
        [JsonIgnore] public int GetIndexPositionsX => IndexPositions.x;
        [JsonIgnore]  public int GetIndexPositionsY => IndexPositions.y;

        [JsonIgnore] public int GetRegionID => RegionID;
        [JsonIgnore] public bool GetIsOccupied => IsOccupied;
        [JsonIgnore] public bool GetCanBeScanned => CanBeScanned;
        
        [JsonIgnore] public Transform GetVisual => Visual;
        [JsonIgnore]  public Renderer GetRenderer => Renderer;
        [JsonIgnore]  public Material GetMaterial => Renderer.material;
        
        [JsonIgnore] public IEnumerable<ILink> GetLinks => Links;
        
        #endregion
    }
}