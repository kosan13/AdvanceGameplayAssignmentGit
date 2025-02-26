using System.Collections.Generic;
using System.Linq;
using Graphs;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using UnityEngine;
using UnityEngine.Rendering;
using static TileSystem.TileSystemFunctions;
using static TileSystem.TileSystemStructs;
using Random = UnityEngine.Random;

namespace Game
{
    public partial class BlobDivisionMaze
    {
        [SerializeField, Range(1,500)] private int maxRoomSize = 5;
        [SerializeField, Range(1, 10)] private int doorCountPerRoom = 2;
        private const float RandomTolerance = 0.1f;
        private void GenerateBlobDivisionMaze()
        {
            ResetRegionID(); // Reset region ID 
            
            (HashSet<Tile>, HashSet<Tile>) subregion = BlobDivisionMazeFloodFill(ResetTilesAndGetSeedTile(Tilemap));
            
            RecursiveFloodFillData recursiveFloodFillData = new (subregion, maxRoomSize);
            RecursiveBlobDivisionMazeFloodFill(recursiveFloodFillData);

            List<Wall> walls = CreatWalls(Tilemap);
            _wallList.AddRange(walls);

            HashSet<(Tile, Tile)>[] gWalls = new HashSet<(Tile, Tile)>[GetRegionID()];
            for (int index = 0; index < gWalls.Length; index++) gWalls[index] = new HashSet<(Tile, Tile)>();
            List<(Tile, Tile)> occupiedWalls = new ();
            
            foreach (Tile tile in Tilemap.GetAllTiles())
            {
                (bool, List<Tile>) neighborsWideDifferentIds = Tilemap.HasNeighborsWideDifferentIds(tile, false);
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

            for (int i = 0; i < doorCountPerRoom; i++)
            {
                gWalls = RemoveEmptyWallGroups(gWalls);
                AddDoors(gWalls);
            }

            CreatOuterWalls();
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
            
            newRecursiveFloodFillData.Item1 = NewRecursiveBlobDivisionMazeFloodFillData(Tilemap, recursiveFloodFillData, recursiveFloodFillData.RecursiveItems.Item1);
            newRecursiveFloodFillData.Item2 = NewRecursiveBlobDivisionMazeFloodFillData(Tilemap, recursiveFloodFillData, recursiveFloodFillData.RecursiveItems.Item2);
            
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
        
        private void CreatOuterWalls()
        {
            foreach (Tile tile in Tilemap.GetTileMap)
            {
                List<Vector3> vertices = new();
                List<Vector2> uv = new();
                List<Color> colors = new();
                List<int> triangles = new();
                Vector3 position = tile.GetWorldPosition;
                
                if (tile.GetIndexPositionsX == 0) CreatWall(new Vector3(position.x - .5f, position.y, position.z), Vector3.back, wallHeight, Color.black, vertices, uv, colors, triangles);
                if (tile.GetIndexPositionsX == tileMapSize.x - 1) CreatWall(new Vector3(position.x + .5f, position.y, position.z), Vector3.forward, wallHeight, Color.black, vertices, uv, colors, triangles);
                if (tile.GetIndexPositionsY == 0) CreatWall(new Vector3(position.x, position.y, position.z - .5f), Vector3.left, wallHeight, Color.black, vertices, uv, colors, triangles);
                if (tile.GetIndexPositionsY == tileMapSize.y - 1) CreatWall(new Vector3(position.x, position.y, position.z + .5f), Vector3.right, wallHeight, Color.black, vertices, uv, colors, triangles);
                
                Mesh mesh = new () { indexFormat = IndexFormat.UInt32, vertices = vertices.ToArray(), uv = uv.ToArray(), colors = colors.ToArray(), triangles = triangles.ToArray() };
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                _wallList.Add(new Wall(mesh, (tile, null)));
            }
        }
        private void AddDoors(HashSet<(Tile, Tile)>[] gWalls)
        {
            foreach (HashSet<(Tile, Tile)> wallGrope in gWalls)
            {
                int randomIndex = Random.Range(0, wallGrope.Count - 1);
                (Tile, Tile) tiles = wallGrope.ElementAt(randomIndex);

                foreach (Wall wall in _wallList.ToArray())
                {
                    if (wall.WallsOwners != tiles && wall.WallsOwners != (tiles.Item2, tiles.Item1)) continue;
                    (Tile, Tile) wallsOwners = wall.WallsOwners;
                    _wallList.Remove(wall);
                    wallsOwners.Item1.AddLink(new Link(wallsOwners.Item1, wallsOwners.Item2));
                    wallsOwners.Item2.AddLink(new Link(wallsOwners.Item2, wallsOwners.Item1));
                    wallGrope.Remove(tiles);
                }
            }
        }
        private static HashSet<(Tile, Tile)>[] RemoveEmptyWallGroups(HashSet<(Tile, Tile)>[] walls)
        {
            int newLength = walls.Count(wall => wall.Count > 0);
            HashSet<(Tile, Tile)>[] newWalls = new HashSet<(Tile, Tile)>[newLength];
            (int i, int j) = (0, 0);
            while (i < walls.Length)
            {
                if (walls[i].Count > 0)
                {
                    newWalls[j] = walls[i]; 
                    j++;
                } 
                i++;
            }
            walls = newWalls;
            return walls;
        }
    }
}