using System.Collections.Generic;
using System.Linq;
using Graphs;
using TileSystem;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using UnityEngine;
using static TileSystem.TileSystemFunctions;
using static TileSystem.TileSystemStructs;

namespace Game
{
    public partial class BlobDivisionMaze
    {
        [SerializeField] private int maxRoomSize = 5;
        private const float RandomTolerance = 0.1f;
        private void GenerateBlobDivisionMaze()
        {
            ResetRegionID(); // Reset region ID 
            
            (HashSet<Tile>, HashSet<Tile>) subregion = BlobDivisionMazeFloodFill(ResetTilesAndGetSeedTile(_tileMap));
            
            RecursiveFloodFillData recursiveFloodFillData = new (subregion, maxRoomSize);
            RecursiveBlobDivisionMazeFloodFill(recursiveFloodFillData);

            List<CombineInstance> combineInstances = CreatWall(_tileMap);
            _combineInstances.AddRange(combineInstances);

            HashSet<(Tile, Tile)>[] gWalls = new HashSet<(Tile, Tile)>[GetRegionID()];
            for (int index = 0; index < gWalls.Length; index++) gWalls[index] = new HashSet<(Tile, Tile)>();
            List<(Tile, Tile)> occupiedWalls = new ();
            
            foreach (Tile tile in _tileMap.GetAllTiles())
            {
                (bool, List<Tile>) neighborsWideDifferentIds = _tileMap.HasNeighborsWideDifferentIds(tile, false);
                foreach (Tile neighborTile in neighborsWideDifferentIds.Item2)
                {
                    foreach (HashSet<(Tile, Tile)> wall in gWalls)
                    {
                        if ((wall.Contains((tile, neighborTile)) || wall.Contains((neighborTile, tile))) && tile.IsSameRegionID(neighborTile)) continue;
                        gWalls[tile.GetRegionID].Add((tile, neighborTile));
                    }
                }
            }
            foreach (HashSet<(Tile, Tile)> wall in gWalls)
            {
                if (wall.Count <= 0) continue;
                List<(Tile, Tile)> listToRemove = new ();
                foreach ((Tile, Tile) tiles in wall)
                {
                    bool duplicate = occupiedWalls.Contains(tiles) || occupiedWalls.Contains((tiles.Item2, tiles.Item1));
                    if (duplicate) listToRemove.Add(tiles);
                    else occupiedWalls.Add(tiles);
                }
                foreach ((Tile, Tile) tiles in listToRemove) wall.Remove(tiles);
            }
            Debug.Log(_combineInstances.Count);

            // foreach (HashSet<(Tile, Tile)> wallList in gWalls)
            // {
            //     foreach ((Tile, Tile) wall in wallList)
            //     { 
            //         Vector3 position = Vector3.Lerp(wall.Item1.GetWorldPosition, wall.Item2.GetWorldPosition, 0.5f);
            //         Transform temp = Instantiate(wall.Item1.GetVisual);
            //         position.y += 8.2f;
            //         temp.localScale = Vector3.one * 0.25f; 
            //         temp.position = position;
            //     }
            // }
        }

        private static (HashSet<Tile>, HashSet<Tile>) BlobDivisionMazeFloodFill((Tile, Tile) start)
        {
            // setup
            (Queue<Tile>, Queue<Tile>) open = (new Queue<Tile>(), new Queue<Tile>());
            (HashSet<Tile>, HashSet<Tile>) closed = (new HashSet<Tile>(), new HashSet<Tile>());
            
            open.Item1.Enqueue(start.Item1);
            open.Item2.Enqueue(start.Item2);

            bool flipper = false;
            // search / iteration
            while (open.Item1.Count > 0 || open.Item2.Count > 0)
            {
                NeighborsSearchData searchData1 = new (open.Item1, closed.Item1, open, closed);
                NeighborsSearchData searchData2 = new (open.Item2, closed.Item2, open, closed);
                if (flipper)
                {
                    if (open.Item1.Count > 0) SearchNeighbors(searchData1, RandomTolerance);
                    if (open.Item2.Count > 0) SearchNeighbors(searchData2, RandomTolerance);
                }
                else
                {
                    if (open.Item2.Count > 0) SearchNeighbors(searchData2, RandomTolerance);
                    if (open.Item1.Count > 0) SearchNeighbors(searchData1, RandomTolerance);
                }
                flipper = !flipper;
            }
            return closed;
        }
        private void RecursiveBlobDivisionMazeFloodFill(RecursiveFloodFillData recursiveFloodFillData)
        {
            (RecursiveFloodFillData, RecursiveFloodFillData) newRecursiveFloodFillData = new (new RecursiveFloodFillData(), new RecursiveFloodFillData());
            
            (HashSet<Tile>, HashSet<Tile>) recursiveItems = recursiveFloodFillData.RecursiveItems;
            int minRoomSize = recursiveFloodFillData.MinRoomSize;

            if (recursiveItems is { Item1: not null, Item2: not null })
            {
                //SetCoolerToVisualiseRegion(recursiveItems,Instantiate(material));
                SetRegionIDOnTiles(recursiveItems);
            }
            
            newRecursiveFloodFillData.Item1 = NewRecursiveBlobDivisionMazeFloodFillData(_tileMap, recursiveFloodFillData, recursiveFloodFillData.RecursiveItems.Item1);
            newRecursiveFloodFillData.Item2 = NewRecursiveBlobDivisionMazeFloodFillData(_tileMap, recursiveFloodFillData, recursiveFloodFillData.RecursiveItems.Item2);
            
            if (recursiveItems.Item1 != null && recursiveItems.Item1.Count > minRoomSize) RecursiveBlobDivisionMazeFloodFill(newRecursiveFloodFillData.Item1);
            if (recursiveItems.Item2 != null && recursiveItems.Item2.Count > minRoomSize) RecursiveBlobDivisionMazeFloodFill(newRecursiveFloodFillData.Item2);
        }
        private static RecursiveFloodFillData NewRecursiveBlobDivisionMazeFloodFillData(TileMapClass tileMap, RecursiveFloodFillData data, HashSet<Tile> recursiveItems)
        {
            RecursiveFloodFillData newRecursiveFloodFillData = new ((null, null), data.MinRoomSize);
            
            if (recursiveItems == null) return newRecursiveFloodFillData;
            if (recursiveItems.Count <= data.MinRoomSize) return newRecursiveFloodFillData;
            
            (HashSet<Tile>, HashSet<Tile>) newRecursiveItems = BlobDivisionMazeFloodFill(ResetTilesAndGetSeedTile(recursiveItems, tileMap));
            
            newRecursiveFloodFillData = new RecursiveFloodFillData(newRecursiveItems, data.MinRoomSize);
            return newRecursiveFloodFillData;
        }
    }
}