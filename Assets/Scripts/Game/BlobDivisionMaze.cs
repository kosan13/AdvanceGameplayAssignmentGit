using System.Collections.Generic;
using Graphs;
using MeshHandlers;
using TileSystem.Tile_Class;
using TileSystem.TileMap_Class;
using UnityEngine;
using UnityEngine.Rendering;
using static TileSystem.TileSystemFunctions;

namespace Game
{
    public partial class BlobDivisionMaze : MeshHandler, ISearchableGraph
    {
        [SerializeField] private Vector2Int tileMapSize = new(100, 100);
        [SerializeField] private float wallHeight = 10.0f;
        [SerializeField] private new Transform camera;
        
        [SerializeField] private Transform visualizer;
        [SerializeField] public Material material;
        
        private TileMapClass _tileMap;
        private List<CombineInstance> _combineInstances = new ();
        
        #region Properties
        public IEnumerable<INode> GetNodes
        {
            get
            {
                if (_tileMap != null)
                {
                    for (int x = 0; x < tileMapSize.x; x++)
                    {
                        for (int y = 0; y < tileMapSize.y; y++)
                        {
                            yield return _tileMap.GetTile(x, y);
                        }
                    }
                }
            }
        }
        #endregion
        
        public float Heuristic(INode start, INode goal) => throw new System.NotImplementedException();
        
        protected void Start()
        {
            InitializedWorld();
            GenerateBlobDivisionMaze();
            MeshFilter.mesh = CreateMesh(_combineInstances);
            MeshRenderer.material = material;
        }

        private void InitializedWorld()
        {
            _tileMap = new TileMapClass(tileMapSize, wallHeight);
            
            //Temp Camera Code
            camera.GetComponent<Camera>().orthographicSize = _tileMap.GetSizeX / 2f + 1;
            camera.position = new Vector3(_tileMap.GetSizeX / 2f, 10, _tileMap.GetSizeY / 2f);
            //
            
            //Create Tiles
            for (int x = 0; x < _tileMap.GetSizeX; x++)
            {
                for (int y = 0; y < _tileMap.GetSizeY; y++)
                {
                    //_tileMap.SetTile(x, y, new Tile(x, y));
                    _tileMap.SetTile(x, y, new Tile(x, y, Instantiate(visualizer)));
                }
            }
            // Create links
            CreateLinks(_tileMap);
             _combineInstances.Add(GenerateBlobDivisionMazeMesh());
        }
        private CombineInstance GenerateBlobDivisionMazeMesh()
        {
            if (_tileMap.IsEmpty) { Debug.LogError("TileMap Length is 0 or empty"); return new CombineInstance(); }
            
            // generate a mesh
            List<Vector3> vertices = new();
            List<Color> colors = new();
            List<int> triangles = new();
            
            for (int x = 0; x < _tileMap.GetSizeX; x++)
            {
                for (int y = 0; y < _tileMap.GetSizeY; y++)
                {
                    int iStart = vertices.Count;
                    vertices.AddRange(new Vector3[] { new(x - 0.5f, 0.0f, y - 0.5f), new(x - 0.5f, 0.0f, y + 0.5f), new(x + 0.5f, 0.0f, y + 0.5f), new(x + 0.5f, 0.0f, y - 0.5f) });
                    Color color = (x + y) % 2 == 0 ? Color.white : Color.black;
                    colors.AddRange(new[] { color, color, color, color });
                    triangles.AddRange(new[] { iStart + 0, iStart + 1, iStart + 2, iStart + 0, iStart + 2, iStart + 3 });
                }
            }
            Mesh mesh = new () { indexFormat = IndexFormat.UInt32, vertices = vertices.ToArray(), colors = colors.ToArray(), triangles = triangles.ToArray() };
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            CombineInstance combineInstance = new () { mesh = mesh };
            return combineInstance;
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
    }
}