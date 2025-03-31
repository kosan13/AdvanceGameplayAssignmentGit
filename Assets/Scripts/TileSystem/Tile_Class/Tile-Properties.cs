using System.Collections.Generic;
using Librarys.Graphs.Interfaces;
using Librarys.Graphs.Scripts;
using UnityEngine;

namespace TileSystem.Tile_Class
{
    public partial class Tile
    {
        #region Properties

        public Vector3 GetWorldPosition => WorldPosition;
        
        public Vector2Int GetIndexPositions => IndexPositions;
        public int GetIndexPositionsX => IndexPositions.x;
        public int GetIndexPositionsY => IndexPositions.y;

        public int GetRegionID => RegionID;
        public bool GetIsOccupied => IsOccupied;
        public bool GetCanBeScanned => CanBeScanned;

        public bool GetVoidArea => VoidArea;
        
        public Transform GetVisual => Visual;
        public Renderer GetRenderer => Renderer;
        public Material GetMaterial => Renderer.material;
        
        // public IEnumerable<ILink> GetLinks => Links;
        public IEnumerable<ILink> GetLinks
        {
            get
            {
                foreach (Link link in Links)
                {
                    yield return link;
                }  
            }
        }

        #endregion

    }
}