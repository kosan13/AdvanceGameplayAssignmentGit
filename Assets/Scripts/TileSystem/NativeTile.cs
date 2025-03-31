using System;
using Librarys.Graphs.Enum;
using TileSystem.Tile_Class;
using Unity.Collections;
using Unity.Mathematics;
using TileSystem.TileMap_Class;
using Unity.Burst;
using UnityEngine;
using static Librarys.Graphs.Enum.Direction;

namespace TileSystem
{
    [BurstCompile]
    public struct NativeTile
    {

        public NativeTile(int2 indexPositions, NativeList<NativeLink> links, bool voidArea = false, int regionID = 0)
        {
            IndexPositions = indexPositions;
            Links = links;
            VoidArea = voidArea;
            RegionID = regionID;
            CanBeScanned = false;
        }
        public NativeTile(int2 indexPositions, bool voidArea = false, int regionID = 0)
        {
            IndexPositions = indexPositions;
            Links = new NativeList<NativeLink>();
            VoidArea = voidArea;
            RegionID = regionID;
            CanBeScanned = false;
        }

        private bool VoidArea { get; set; }
        private int2 IndexPositions { get; set; }
        private int RegionID { get; set; }
        private bool CanBeScanned { get; set; }
        private NativeList<NativeLink> Links { get; set; }


        public int2 GetIndexPositions => IndexPositions;
        public int GetIndexPositionsX => IndexPositions.x;
        public int GetIndexPositionsY => IndexPositions.y;

        public int GetRegionID => RegionID;
        public bool GetCanBeScanned => CanBeScanned;

        public NativeList<NativeLink> GetLinks => Links;
        
        public NativeList<NativeLink> SetLinksList(NativeList<NativeLink> linksList) => Links = linksList;
        public NativeList<NativeLink> SetLinksListToNew() => Links = new NativeList<NativeLink>();
        
        public void AddLink(NativeLink link) => Links.Add(link);
        public void RemoveLink(NativeLink link)
        {
            int index = -1;
            for (int i = 0; i < Links.Length - 1; i++)
            {
                if (Links[i] != link) continue;
                index = i;
                break;
            }
            if (index == -1) return;
            Links.RemoveAt(index);
        }
        
        public int SetRegionID(int regionID) => RegionID = regionID;
        public bool SetCanBeScanned(bool scanned) => CanBeScanned = scanned; 
        
        public NativeTile GetNeighbour(ref NativeArray<NativeArrayContainer> worldMap, Direction direction)
        {
            int2 neighbourJaggedIndex = direction switch
            {
                Null or Up or Down => IndexPositions,
                Forward or North => IndexPositions + new int2(-1, 0),
                Back or South => IndexPositions + new int2(1, 0),
                Right or East => IndexPositions + new int2(0, 1),
                Left or West => IndexPositions + new int2(0, -1),
                Northeast => IndexPositions + new int2(1, 1),
                Southeast => IndexPositions + new int2(-1, 1),
                Northwest => IndexPositions + new int2(1, -1),
                Southwest => IndexPositions + new int2(-1, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (neighbourJaggedIndex.x >= worldMap.Length || neighbourJaggedIndex.y >= worldMap.Length) return this;
            if (neighbourJaggedIndex.x < 0 || neighbourJaggedIndex.y < 0) return this;
            
            NativeTile tile = worldMap[neighbourJaggedIndex.x].Tiles[neighbourJaggedIndex.y];
            return tile.VoidArea ? this : tile;
        }
        public Tile GetNeighbour(TileMapClass tilemap, Direction direction)
        {
            int2 neighbourJaggedIndex = direction switch
            {
                Direction.Null or Up or Down => IndexPositions,
                Forward or North => IndexPositions + new int2(1, 0),
                Back or South => IndexPositions + new int2(-1, 0),
                Right or East => IndexPositions + new int2(0, 1),
                Left or West => IndexPositions + new int2(0, -1),
                Northeast => IndexPositions + new int2(1, 1),
                Southeast => IndexPositions + new int2(-1, 1),
                Northwest => IndexPositions + new int2(1, -1),
                Southwest => IndexPositions + new int2(-1, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (neighbourJaggedIndex.x >= tilemap.GetLength || neighbourJaggedIndex.y >= tilemap.GetLength) return null;
            if (neighbourJaggedIndex.x < 0 || neighbourJaggedIndex.y < 0) return null;

            Tile tile = tilemap.GetTile(neighbourJaggedIndex.x,neighbourJaggedIndex.y);
            if (tile is null) return null;
            return tile.GetVoidArea ? null : tile;
        }
        public Vector2Int GetNeighbourIndex(TileMapClass tilemap, Direction direction)
        {
            Tile neighbour = GetNeighbour(tilemap, direction);
            return neighbour?.GetIndexPositions ?? new Vector2Int(-1, -1);
        }

        #region operator
        
        public bool Equals(NativeTile other) => VoidArea == other.VoidArea && IndexPositions.Equals(other.IndexPositions) && RegionID == other.RegionID;
        public override bool Equals(object obj) => obj is NativeTile other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(VoidArea, IndexPositions, RegionID);
        
        public static bool operator == (NativeTile tileOne, NativeTile tileTwo)
        {
            bool2 check = tileOne.GetIndexPositions == tileTwo.GetIndexPositions;
            return check is { x: true, y: true };
        }
        public static bool operator == (int2 value, NativeTile tileTwo)
        {
            bool2 check = value == tileTwo.GetIndexPositions;
            return check is { x: true, y: true };
        }
        
        public static bool operator != (NativeTile tileOne, NativeTile tileTwo) => !(tileOne == tileTwo);
        public static bool operator !=(int2 value, NativeTile tileTwo) => !(value == tileTwo);
        
        #endregion
        #region static
            
        public static bool IsSameRegionID(NativeTile tileOne, NativeTile tileTwo) => tileOne.RegionID == tileTwo.RegionID;
            
        #endregion
    }

    [BurstCompile]
    public struct NativeLink
    {
        #region Properties
        public int2 Source { get; }
        public int2 Target { get; }
        public Direction Direction { get; private set; }
        #endregion
        
        public NativeLink(int2 source, int2 target)
        {
            Source = source;
            Target = target;
            Direction = GetLinkDirection(source, target);
        }
        public NativeLink(int2 source, int2 target, Direction direction = Direction.Null)
        {
            Source = source;
            Target = target;
            Direction = direction;
        }
        
        public static Direction GetLinkDirection(NativeLink link , int2 source, int2 target) => link.Direction = GetLinkDirection(source, target);
        public static Direction GetLinkDirection(int2 source, int2 target)
        {
            Direction direction = Direction.Null;
            if (source.x < target.x) { direction = Forward; }
            if (source.x > target.x) { direction = Back; }
            if (source.y < target.y) { direction = Right; }
            if (source.y > target.y) { direction = Left; }
            return direction;
        }

        public static NativeList<NativeLink> GetAllValidLinks(TileMapClass tilemap ,NativeTile nativeTile)
        {
            //value for the Error Check in the checkIndex if Statement
            int2 errorValue = new (-1, -1);
            NativeList<NativeLink> links = new (Allocator.Persistent);
            int2 neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Forward));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile)
            {
                NativeLink l = new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y), Forward);
                links.Add(l);
            } 
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Back));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile)
            {
                NativeLink l = new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y), Back);
                links.Add(l);
            }
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Left));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Left));
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Right));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Right));
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Northeast));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Northeast));
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Southeast));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Southeast));
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Northwest));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Northwest));
            neighbourIndex = Vector2IntToInt2(nativeTile.GetNeighbourIndex(tilemap, Southwest));
            if (BoolTwoToBool(neighbourIndex != errorValue) && neighbourIndex != nativeTile) links.Add(new NativeLink(nativeTile.GetIndexPositions, new int2(neighbourIndex.x, neighbourIndex.y) ,Southwest));
            return links;
            int2 Vector2IntToInt2(Vector2Int value) => new (value.x, value.y);
            bool BoolTwoToBool(bool2 value) => value is { x: true, y: true };
        }
        
        #region operator
        
        public bool Equals(NativeLink other) => Source.Equals(other.Source) && Target.Equals(other.Target) && Direction == other.Direction;
        public override bool Equals(object obj) => obj is NativeLink other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Source, Target, (int)Direction);
        
        public static bool operator == (NativeLink linkOne, NativeLink linkTwo)
        {
            bool2 check = linkOne.Source == linkTwo.Source;
            return check is { x: true, y: true };
        }
        public static bool operator != (NativeLink linkOne, NativeLink linkTwo) => !(linkOne == linkTwo);
        
        #endregion
    }

    [BurstCompile]
    public struct NativeArrayContainer
    {
        public NativeArray<NativeTile> Tiles;
        public NativeArrayContainer(NativeArray<NativeTile> tiles) => Tiles = tiles;
    }
    
    // public struct NativeTile : INativePositionNode
    // {
    //     private float3 WorldPosition { get; set; }
    //     private int2 IndexPositions { get; set; }
    //     private NativeList<NativeLink> Links { get; set; }
    //     private bool IsOccupied { get; set; }
    //     private bool CanBeScanned { get; set; }
    //
    //     #region Properties
    //
    //     public float3 GetWorldPosition => WorldPosition;
    //     
    //     public int2 GetIndexPositions => IndexPositions;
    //     public int GetIndexPositionsX => IndexPositions.x;
    //     public int GetIndexPositionsY => IndexPositions.y;
    //
    //     public int GetRegionID => RegionID;
    //     public bool GetIsOccupied => IsOccupied;
    //     public bool GetCanBeScanned => CanBeScanned;
    //     
    //     public NativeList<NativeLink> GetLinks => Links;
    //     
    //     #endregion
    //     
    //     
    //     public NativeList<NativeLink> SetLinksList(NativeList<NativeLink> linksList) => Links = linksList;
    //     public NativeList<NativeLink> SetLinksListToNew() => Links = new NativeList<NativeLink>();
    //     
    //     public void AddLink(NativeLink link) => Links.Add(link);
    //     public void RemoveLink(NativeLink link)
    //     {
    //         int index = -1;
    //         for (int i = 0; i < Links.Length - 1; i++)
    //         {
    //             if (Links[i] != link) continue;
    //             index = i;
    //             break;
    //         }
    //         if (index == -1) return;
    //         Links.RemoveAt(index);
    //     }
    //
    //     public int SetRegionID(int regionID) => RegionID = regionID;
    //     public bool SetIsOccupied(bool occupied) => IsOccupied = occupied; 
    //     public bool SetCanBeScanned(bool scanned) => CanBeScanned = scanned; 
    //     
    //     public bool IsSameTile(NativeTile tile) => this == tile;
    //     public bool IsSameRegionID(NativeTile tile) => tile.RegionID == RegionID;
    //
    //     public bool IsNeighbors(NativeTile tile, bool checkCorners = true)
    //     {
    //         if (GetIndexPositionsY == tile.GetIndexPositionsX + 1) return true;
    //         if (GetIndexPositionsY == tile.GetIndexPositionsX - 1) return true;
    //         if (GetIndexPositionsX == tile.GetIndexPositionsY + 1) return true;
    //         if (GetIndexPositionsX == tile.GetIndexPositionsY - 1) return true;
    //
    //         if (!checkCorners) return false;
    //         
    //         if (GetIndexPositionsX - 1 == tile.GetIndexPositionsY - 1) return true;
    //         if (GetIndexPositionsY + 1 == tile.GetIndexPositionsX + 1) return true;
    //         if (GetIndexPositionsX - 1 == tile.GetIndexPositionsY + 1) return true;
    //         if (GetIndexPositionsY - 1 == tile.GetIndexPositionsX + 1) return true;
    //         return false;
    //     }
    //     
    //     #region static
    //         
    //     public static bool IsSameRegionID(NativeTile tileOne, NativeTile tileTwo) => tileOne.RegionID == tileTwo.RegionID;
    //         
    //     #endregion
    //     
    //     #region operator
    //     
    //     public bool Equals(NativeTile other) => IndexPositions.Equals(other.IndexPositions);
    //     public override bool Equals(object obj) => obj is NativeTile other && Equals(other);
    //     public override int GetHashCode() => IndexPositions.GetHashCode();
    //     
    //     public static bool operator == (NativeTile tileOne, NativeTile tileTwo)
    //     {
    //         bool2 check = tileOne.IndexPositions == tileTwo.IndexPositions;
    //         return check is { x: true, y: true };
    //     }
    //     public static bool operator !=(NativeTile tileOne, NativeTile tileTwo) => !(tileOne == tileTwo);
    //     
    //     #endregion
    // }
}