using System;
using System.Collections.Generic;
using System.Linq;
using Librarys.Graphs.Interfaces;
using Librarys.MeshHandlers.Scripts;
using TileSystem.TileMap_Class;
using UnityEngine;
using UnityEngine.Rendering;
using static TileSystem.TileSystemFunctions;
using static TileSystem.TileSystemStructs;
using static Librarys.Graphs.Scripts.GraphAlgorithms;
using Tile = TileSystem.Tile_Class.Tile;

namespace Game
{
    public partial class BlobDivisionMaze : MeshHandler, ISearchableGraph
    {
        [SerializeField] private Vector2Int tileMapSize = new(100, 100);
        [SerializeField] private float wallHeight = 10.0f;
        
        [SerializeField] private Shader floorShader;
        [SerializeField] private Shader wallShader;
        [SerializeField] private Texture2D floorTexture;

        public static bool LodeGameBole { get; set; } = false;
        private CombineInstance _floor;
        private List<Wall> _wallList = new ();
        private List<CombineInstance> _combineInstances = new ();
        
        #region Properties
        public TileMapClass Tilemap { get; private set; }

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
        public HashSet<Tile> TileLevel { get; private set; }
        public static BlobDivisionMaze Instance { get; private set; }
        
        #endregion
        
        public float Heuristic(INode start, INode goal) => throw new NotImplementedException();

        protected override void OnEnable()
        {   
            base.OnEnable();
            Instance = this;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Instance = Instance == this ? null : Instance;
        }

        protected void Start()
        {
            if (LodeGameBole) LodeGame();
            else StartGame();
        }

        private void StartGame()
        {
            DateTime start = DateTime.Now;
            
            InitializedWorld();
            GenerateBlobDivisionMaze();
            
            Debug.Log( $"world calc is {DateTime.Now.Subtract(start).TotalSeconds}");
            
            start = DateTime.Now;
            
            _combineInstances.Add(_floor);
            foreach (Wall wall in _wallList) _combineInstances.Add(new CombineInstance { mesh = wall.Mesh});
            MeshFilter.mesh = CreateMesh(_combineInstances);
            AddMaterialToMeshes();
            
            Debug.Log( $"mesh calc is {DateTime.Now.Subtract(start).TotalSeconds}");
            
            TileLevel = LongestFloodFill();
            Debug.Log(TileLevel.ToArray().Length);
            Level.CreateLevelAndAddLevel(gameObject);
        }
        private void LodeGame()
        {
            // Tilemap = SaveSystem.SaveSystem.SaveFileData.Tilemap;
            // TileLevel = SaveSystem.SaveSystem.SaveFileData.Level;
            // MeshFilter.mesh = SaveSystem.SaveSystem.SaveFileData.WorldMap;
            // PlayerCharacter.SetAndInstantiatePlayerCharacterInstance((PlayerCharacter)SaveSystem.SaveSystem.SaveFileData.PlayerCharacter);
            // PlayerCharacter.Instance.transform.position = PlayerCharacter.Instance.WorldPosition;
            // Level.AddLevel(gameObject);
            // Level.TurnOrder = SaveSystem.SaveSystem.SaveFileData.TurnOrder;
        }

        private void InitializedWorld()
        {
            Tilemap = new TileMapClass(tileMapSize, wallHeight);
            // Create Tiles
            for (int x = 0; x < Tilemap.GetSizeX; x++)
                for (int y = 0; y < Tilemap.GetSizeY; y++)
                    Tilemap.SetTile(x, y, new Tile(x, y));
            // Create links
            CreateLinks(Tilemap);
            // Create Ground Mesh
            _floor = GenerateBlobDivisionMazeMesh();
        }
        private void AddMaterialToMeshes()
        {
            Material[] subMeshMaterials = new Material[MeshFilter.mesh.subMeshCount];
            Material floorMaterial = new (floorShader)
            {
                mainTexture = floorTexture,
                mainTextureScale = Vector2.one / 2,
                mainTextureOffset = Vector2.one / 4
            };
            subMeshMaterials[0] = floorMaterial;
            
            Material wallMaterial = new (wallShader);
            for (int i = 1; i < subMeshMaterials.Length; i++) subMeshMaterials[i] = wallMaterial;
            MeshRenderer.materials = subMeshMaterials;
        }
        private CombineInstance GenerateBlobDivisionMazeMesh()
        {
            if (Tilemap.IsEmpty) { Debug.LogError("TileMap Length is 0 or empty"); return new CombineInstance(); }
            
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
            List<Vector2> uv = vertices.Select(v => new Vector2(Vector3.Dot(Vector3.forward, v), Vector3.Dot(Vector3.right, v))).ToList();
            return new CombineInstance { mesh = NewMesh(vertices, uv, colors, triangles)};
        }
        private static Mesh CreateMesh(List<CombineInstance> combineInstances)
        {
            Mesh mesh = new () { indexFormat = IndexFormat.UInt32, };
            mesh.CombineMeshes(combineInstances.ToArray(), false, false);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();
            return mesh;
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
    }
}