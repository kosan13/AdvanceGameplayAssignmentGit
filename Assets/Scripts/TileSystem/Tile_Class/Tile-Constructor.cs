using System.Collections.Generic;
using Librarys.Graphs.Scripts;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace TileSystem.Tile_Class
{
    public partial class Tile
    {
        #region Constructor
        
        public Tile(int indexPositionX, int indexPositionY)
        {
            WorldPosition = new Vector3Int(indexPositionX, 0, indexPositionY);
            IndexPositions = new Vector2Int(indexPositionX, indexPositionY);
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            Visual = null;
            Renderer = null;
        }
        public Tile(int indexPositionX, int indexPositionY, Transform visual)
        {
            WorldPosition = new Vector3Int(indexPositionX, 0, indexPositionY);
            IndexPositions = new Vector2Int(indexPositionX, indexPositionY);
            Visual = visual;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            
            Renderer = Visual.GetComponent<Renderer>();
            Visual.position = WorldPosition;
        }
        public Tile(Vector2Int indexPosition)
        {
            WorldPosition = new Vector3Int(indexPosition.x, 0, indexPosition.y);
            IndexPositions = indexPosition;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            Visual = null;
            Renderer = null;
        }
        public Tile(Vector2Int indexPosition, Transform visual)
        {
            WorldPosition = new Vector3Int(indexPosition.x, 0, indexPosition.y);
            IndexPositions = indexPosition;
            Visual = visual;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            
            Renderer = Visual.GetComponent<Renderer>();
            Visual.position = WorldPosition;
        }
        public Tile(int indexPositionX, int indexPositionY, int worldPositionX, int worldPositionY, int worldPositionZ)
        {
            WorldPosition = new Vector3Int(worldPositionX, worldPositionY, worldPositionZ);
            IndexPositions = new Vector2Int(indexPositionX, indexPositionY);
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            Visual = null;
            Renderer = null;
        }
        public Tile(int indexPositionX, int indexPositionY, int worldPositionX, int worldPositionY, int worldPositionZ, Transform visual)
        {
            WorldPosition = new Vector3Int(worldPositionX, worldPositionY, worldPositionZ);
            IndexPositions = new Vector2Int(indexPositionX, indexPositionY);
            Visual = visual;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            
            Renderer = Visual.GetComponent<Renderer>();
            Visual.position = WorldPosition;
        }
        public Tile(Vector2Int indexPosition, Vector3Int worldPosition)
        {
            WorldPosition = worldPosition;
            IndexPositions = indexPosition;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            Visual = null;
            Renderer = null;
        }
        public Tile(Vector2Int indexPosition, Vector3Int worldPosition, Transform visual)
        {
            WorldPosition = worldPosition;
            IndexPositions = indexPosition;
            Visual = visual;
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            
            Renderer = Visual.GetComponent<Renderer>();
            Visual.position = WorldPosition;
        }
        public Tile(Tile tile)
        {
            WorldPosition = tile.WorldPosition;
            IndexPositions = tile.IndexPositions;
            Visual = tile.Visual;
            Links = tile.Links;
            IsOccupied = tile.IsOccupied;
            CanBeScanned = tile.CanBeScanned;
            RegionID = tile.RegionID;
            
            Renderer = Visual.GetComponent<Renderer>();
        }
        public Tile()
        {
            WorldPosition = new Vector3Int(0, 0, 0);
            IndexPositions = new Vector2Int(0, 0);
            Links = new List<Link>();
            IsOccupied = false;
            CanBeScanned = true;
            Visual = new RectTransform();
        }
        
        #endregion
    }
}