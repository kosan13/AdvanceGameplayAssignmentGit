using System;
using System.Collections.Generic;
using System.Linq;
using Librarys.Graphs.Interfaces;
using Librarys.MeshHandlers.Scripts;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using UnityEngine;
using static Librarys.Graphs.Scripts.GraphAlgorithms;
using static TileSystem.TileSystemFunctions;
using static TileSystem.TileSystemStructs;

namespace Game
{
    public partial class Maze : MeshHandlerInstance, IGraph
    {
        [Header("TileMap")]
        [SerializeField] private Vector2Int tileMapSize = new(100, 100);
        [SerializeField] private int wallHeight = 10;
        
        [Header("Floor visuals")]
        [SerializeField] private Shader floorShader;
        [SerializeField] private TextureEditorRaper floorTexture;
        
        [Header("Wall visuals")]
        [SerializeField] private Shader wallShader;

        public static bool LodeGameBole { get; set; } = false;
        
        private CombineInstance _floorMesh;
        private List<CombineInstance> _worldMesh = new ();
        private List<Wall> _wallList = new ();
        
        #region Properties
        
        public TileMapClass Tilemap { get; private set; }
        public HashSet<Tile> TileLevel { get; private set; }
        public IEnumerable<INode> GetNodes
        {
            get
            {
                if (Tilemap != null)
                {
                    for (int x = 0; x < tileMapSize.x; x++)
                    {
                        for (int y = 0; y < tileMapSize.y; y++)
                        {
                            yield return Tilemap.GetTile(x, y);
                        }
                    }
                }
            }
        }
        
        #endregion
        
        protected void Start()
        {
            if (LodeGameBole) LodeGame();
            else StartGame();
        }
        
        private void StartGame()
        {
            DateTime start = DateTime.Now;
            InitializedWorld();
            GenerateMaze();
            
            double fTotalTime = DateTime.Now.Subtract(start).TotalSeconds;
            Debug.Log( $"world calc is {fTotalTime}");
            
            // start = DateTime.Now;
            // //Create WorldMesh
            // CreateWorldMesh();
            //
            // fTotalTime = DateTime.Now.Subtract(start).TotalSeconds;
            // Debug.Log( $"mesh calc is {fTotalTime}");
            
            //Get the Level
            // TileLevel = LongestFloodFill();
            // DebugLog(TileLevel.ToArray().Length, "Length of Level is");
            // Level.CreateLevelAndAddLevel(gameObject);
        }

        private void LodeGame() => StartGame();
        private void InitializedWorld()
        {
            // Create Tilemap and Tiles
            Tilemap = new TileMapClass(tileMapSize, wallHeight, true);
            // Create links
            CreateLinks(Tilemap);
            // Create Ground Mesh
            _floorMesh = GenerateFloorMesh();
            // Create the CalcTileMap
            GenerateNativeTileMapData();
        }
        private CombineInstance GenerateFloorMesh()
        {
            // generate a mesh
            List<Vector3> vertices = new();
            List<Color> colors = new();
            List<int> triangles = new();
            
            for (int x = 0; x < Tilemap.GetSizeX; x++)
            {
                for (int y = 0; y < Tilemap.GetSizeY; y++)
                {
                    int iStart = vertices.Count;
                    vertices.AddRange(new Vector3[] { new(x - 0.5f, 0.0f, y - 0.5f), new(x - 0.5f, 0.0f, y + 0.5f), new(x + 0.5f, 0.0f, y + 0.5f), new(x + 0.5f, 0.0f, y - 0.5f) });
                    Color color = (x + y) % 2 == 0 ? Color.white : Color.black;
                    colors.AddRange(new[] { color, color, color, color });
                    triangles.AddRange(new[] { iStart + 0, iStart + 1, iStart + 2, iStart + 0, iStart + 2, iStart + 3 });
                }
            }
            // calculate uvs (planar mapping)
            List<Vector2> uv = vertices.Select(vector3 => new Vector2(Vector3.Dot(Vector3.forward, vector3), Vector3.Dot(Vector3.right, vector3))).ToList();
            return new CombineInstance { mesh = NewMesh(vertices, uv, colors, triangles)};
        }
        private void CreateWorldMesh()
        {
            _worldMesh.Add(_floorMesh);
            foreach (Wall wall in _wallList) _worldMesh.Add(new CombineInstance { mesh = wall.Mesh});
            MeshFilter.mesh = NewMesh(_worldMesh);
            AddMaterialToMeshes();
        }
        private void AddMaterialToMeshes()
        {
            Material[] subMeshMaterials = new Material[MeshFilter.mesh.subMeshCount];
            subMeshMaterials[0] = new Material (floorShader)
            {
                mainTexture = floorTexture.texture2D,
                mainTextureScale = Vector2.one / floorTexture.textureScale,
                mainTextureOffset = Vector2.one / floorTexture.textureOffset
            };
            for (int i = 1; i < subMeshMaterials.Length; i++) subMeshMaterials[i] = new Material (wallShader);
            MeshRenderer.materials = subMeshMaterials;
        }
        private HashSet<Tile> LongestFloodFill()
        {
            HashSet<Tile> longestFloodFill = new();
            List<HashSet<Tile>> floodFillRegion = new();
            foreach (HashSet<Tile> tileRegion in Tilemap.AllRegion(GetRegionID()))
            {
                bool contains = floodFillRegion.Any(region => region.Contains(tileRegion.ElementAt(0)));
                if (contains) continue;
                floodFillRegion.Add(FloodFill(tileRegion.ElementAt(0)));
            }
            foreach (HashSet<Tile> region in floodFillRegion.Where(region => longestFloodFill.Count <= region.Count)) longestFloodFill = region;
            return longestFloodFill;
        }

        
        [Serializable]
        public struct TextureEditorRaper
        {
            [Header("Texture")]
            public Texture2D texture2D;
            public int textureScale;
            public int textureOffset;
        }
    }
} 