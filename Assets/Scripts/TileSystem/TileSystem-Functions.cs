using System.Collections.Generic;
using System.Linq;
using Enums;
using Graphs;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

using static TileSystem.TileSystemStructs;

namespace TileSystem
{
    public static class TileSystemFunctions
    {
        private static readonly int NewColor = Shader.PropertyToID("_Color");
        private static int _regionID;

        public static int GetRegionID() => _regionID;
        public static int SetRegionID(int regionID) => _regionID = regionID;
        public static int ResetRegionID() => SetRegionID(0);
        public static int IncrementRegionIDByOne() => SetRegionID(GetRegionID() + 1);
        
        public static void CreateLinks(TileMapClass tileMap)
        {
            if (tileMap.IsEmpty) { Debug.LogError("TileMap Length is 0 or empty"); return; }
            for (int x = 0; x < tileMap.GetSizeX; x++)
            {
                for (int y = 0; y < tileMap.GetSizeY; y++)
                {
                    for (int x1 = -1; x1 <= 1; x1++)
                    {
                        for (int y1 = -1; y1 <= 1; y1++)
                        {
                            Vector2Int v = new(x + x1, y + y1);
                            if (v.x < 0 || v.y < 0 || v.x >= tileMap.GetSizeX || v.y >= tileMap.GetSizeY) continue;
                            Tile tile1A = tileMap.GetTile(x, y);
                            Tile tile1B = tileMap.GetTile(v);
                            if (!tile1A.IsSameTile(tile1B)) tile1A.AddLink(new Link(tile1A, tile1B, Link.GetLinkDirection( tile1A, tile1B)));
                        }
                    }
                }
            }
        }
        public static (bool, Tile) IsValidScan(INode node, (Queue<Tile>, Queue<Tile>) open, (HashSet<Tile>, HashSet<Tile>) closed)
        {
            Tile neighbor = null;

            if (node is Tile tile) neighbor = tile;
            if (neighbor is null) return (false, null);

            bool case1 = !open.Item1.Contains(neighbor) && !closed.Item1.Contains(neighbor);
            bool case2 = !open.Item2.Contains(neighbor) && !closed.Item2.Contains(neighbor);
            bool isValidScan = case1 && case2 && neighbor.GetCanBeScanned;

            return (isValidScan, neighbor);
        }
        public static void SearchNeighbors(NeighborsSearchData searchData, float randomTolerance = 0.0f)
        {
            bool guarantee = false;
            Tile node = searchData.Open.Dequeue();
            
            searchData.Closed.Add(node);
            node.SetCanBeScanned(false);
            
            // search the neighbors
            foreach (ILink link in node.GetLinks)
            {
                if (link.Source is Tile temp1 && link.Target is Tile temp2 && (temp1.GetIndexPositionsX != temp2.GetIndexPositionsX && temp1.GetIndexPositionsY != temp2.GetIndexPositionsY)) continue;
                float random = Random.Range(0f, 1f);
                (bool isValidScan, Tile neighbor) = IsValidScan(link.Target, searchData.OpenQueue, searchData.ClosedHashSet);
                if (!isValidScan) continue;
                if (guarantee)
                {
                    searchData.Open.Enqueue(neighbor);
                    guarantee = false;
                }
                else if (random < randomTolerance) searchData.Open.Enqueue(neighbor);
                else
                {
                    searchData.Open.Enqueue(node);
                    guarantee = true;
                }
            }
        }
        public static (Tile, Tile) ResetTilesAndGetSeedTile(HashSet<Tile> tiles, TileMapClass tileMap)
        {
            (Vector2Int, Vector2Int) seed;
            (Tile, Tile) seedTiles;
            
            //Set the Tiles to become scannable
            foreach (Tile tile in tiles) tile.SetCanBeScanned(true);
            
            //Get the seeds
            seed.Item1 = tiles.ToArray()[Random.Range(0, tiles.Count)].GetIndexPositions;
            seed.Item2 = tiles.ToArray()[Random.Range(0, tiles.Count)].GetIndexPositions;
            while (seed.Item1 == seed.Item2) seed.Item2 = tiles.ToArray()[Random.Range(0, tiles.Count)].GetIndexPositions;
            
            //Get the seedTiles
            seedTiles.Item1 = tileMap.GetTile(seed.Item1);
            seedTiles.Item2 = tileMap.GetTile(seed.Item2);
            return seedTiles;
        }
        public static (Tile, Tile) ResetTilesAndGetSeedTile(TileMapClass tiles)
        {
            (Vector2Int, Vector2Int) seed;
            (Tile, Tile) seedTiles;
            
            //Set the Tiles to become scannable
            foreach (Tile tile in tiles.GetAllTiles()) { tile.SetCanBeScanned(true); }
            
            //Get the seeds
            seed.Item1 = new Vector2Int(Random.Range(0, tiles.GetSizeX), Random.Range(0, tiles.GetSizeY));
            seed.Item2 = new Vector2Int(Random.Range(0, tiles.GetSizeX), Random.Range(0, tiles.GetSizeY));
            while (seed.Item1 == seed.Item2) seed.Item2 = new Vector2Int(Random.Range(0, tiles.GetSizeX), Random.Range(0, tiles.GetSizeY));
            
            //Get the seedTiles
            seedTiles.Item1 = tiles.GetTile(seed.Item1);
            seedTiles.Item2 = tiles.GetTile(seed.Item2);
            return seedTiles;
        }
        public static void SetCoolerToVisualiseRegion((HashSet<Tile>, HashSet<Tile>) tileSets, Material material)
        {
            float random = Random.Range(0, 1f);
            Color color = Color.HSVToRGB(random, 1, 0.7f);
            
            foreach (Tile tile in tileSets.Item1)
            {
                material.SetColor(NewColor ,color);
                tile.SetMaterial(material);
            }
            foreach (Tile tile in tileSets.Item2)
            {
                material.SetColor(NewColor ,color);
                tile.SetMaterial(material);
            }
        }
        public static void SetRegionIDOnTiles((HashSet<Tile>, HashSet<Tile>) tileSets)
        {
            foreach (Tile tile in tileSets.Item1) { tile.SetRegionID(GetRegionID()); }
            IncrementRegionIDByOne();
            foreach (Tile tile in tileSets.Item2) { tile.SetRegionID(GetRegionID()); }
            IncrementRegionIDByOne();
        }
        
        public static List<Wall> CreatWalls(TileMapClass tileMap)
        {
            List<Wall> walls = new();
            for (int x = 0; x < tileMap.GetSizeX; x++)
            {
                for (int y = 0; y < tileMap.GetSizeY; y++)
                {
                    List<Link> linkList = new ();
                    HashSet<Link> linkHashSet = new ();
                    
                    Tile tile = tileMap.GetTile(x, y);
                    foreach (ILink iLink in tile.GetLinks)
                    {
                        if (iLink.Source is not Tile tempOne || iLink.Target is not Tile tempTwo || tempOne.GetRegionID == tempTwo.GetRegionID) continue;
                        Link link = (Link)iLink;
                        linkList.Add((Link)iLink);
                        
                        if (iLink.Source is Tile temp1 && iLink.Target is Tile temp2 && (temp1.GetIndexPositionsX != temp2.GetIndexPositionsX && temp1.GetIndexPositionsY != temp2.GetIndexPositionsY)) continue;

                        linkHashSet.Add(link);
                    }
                    foreach (Link link in linkHashSet)
                    {
                        List<Vector3> vertices = new();
                        List<Vector2> uv = new();
                        List<Color> colors = new();
                        List<int> triangles = new();
                        Vector3 tempVector3 = Vector3.Lerp(((Tile)link.Source).GetWorldPosition, ((Tile)link.Target).GetWorldPosition, 0.5f);
                        float wallHeight = tileMap.GetWallHeight;
                        switch(link.Direction)
                        {
                            case Direction.Null: Debug.LogError("LinkDirection Is Null"); break;
                            case Direction.DirectionUp: Debug.LogError("LinkDirection Is DirectionUp"); break;
                            case Direction.DirectionDown: Debug.LogError("LinkDirection Is DirectionDown"); break;
                            case Direction.DirectionForward: CreatWall(tempVector3, Vector3.forward, wallHeight, Color.black, vertices, uv, colors, triangles); break;
                            case Direction.DirectionBack: CreatWall(tempVector3, Vector3.back, wallHeight, Color.black, vertices, uv, colors, triangles); break;
                            case Direction.DirectionLeft: CreatWall(tempVector3, Vector3.left, wallHeight, Color.black, vertices, uv, colors, triangles); break;
                            case Direction.DirectionRight: CreatWall(tempVector3, Vector3.right, wallHeight, Color.black, vertices, uv, colors, triangles); break;
                            default: Debug.LogError("LinkDirection Is Null"); break;
                        }
                        Mesh mesh = new () { indexFormat = IndexFormat.UInt32, vertices = vertices.ToArray(), uv = uv.ToArray(), colors = colors.ToArray(), triangles = triangles.ToArray() };
                        mesh.RecalculateBounds();
                        mesh.RecalculateNormals();
                        walls.Add(new Wall(mesh, ((Tile)link.Source, (Tile)link.Target)));
                    }
                    foreach (Link link in linkList) tile.RemoveLink(link);
                }
            }
            return walls;
        }
        public static void CreatWall(Vector3 vPosition, Vector3 vDirection, float wallHeight, Color color, List<Vector3> vertices, List<Vector2> uv, List<Color> colors, List<int> triangles)
        {
            CreatQuad(vPosition, vDirection, wallHeight, color, vertices, uv, colors, triangles);
            CreatQuad(vPosition, -vDirection, wallHeight, color, vertices, uv, colors, triangles);
        }

        public static void CreatQuad(Vector3 vPosition, Vector3 vDirection, float wallHeight, Color color, List<Vector3> vertices, List<Vector2> uv, List<Color> colors, List<int> triangles)
        {
            Vector3 vRight = Vector3.Cross(vDirection, Vector3.up).normalized;
            float angel = Mathf.Atan2(vRight.z, vRight.x) * Mathf.Rad2Deg + 90;
            float radians = math.radians(angel);
            vRight.x = math.cos(radians);
            vRight.z = math.sin(radians);
            // calculate vertices
            int iStart = vertices.Count;
            vPosition += Vector3.up;
            vertices.AddRange(new Vector3[]
            {
                vPosition - vRight * 0.5f - Vector3.up,
                vPosition - vRight * 0.5f + Vector3.up * wallHeight,
                vPosition + vRight * 0.5f + Vector3.up * wallHeight,
                vPosition + vRight * 0.5f - Vector3.up
            });

            // calculate uvs (planar mapping)
            for (int i = 0; i < 4; ++i)
            {
                Vector3 v = vertices[iStart + i];
                uv.Add(new Vector2(Vector3.Dot(vRight, v), Vector3.Dot(Vector3.up, v)));
            }

            // add colors
            colors.AddRange(new [] { color, color, color, color });
            
            // add triangles
            triangles.AddRange(new []
            {
                iStart + 0, iStart + 1, iStart + 2,
                iStart + 0, iStart + 2, iStart + 3,
            });
        }
    }
}