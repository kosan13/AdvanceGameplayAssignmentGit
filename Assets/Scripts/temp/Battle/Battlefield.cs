// using Graphs;
// using Math;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Tilemaps;
//
// namespace Game.Battle
// {
//     [RequireComponent(typeof(MeshFilter))]
//     [RequireComponent(typeof(MeshRenderer))]
//     public class Battlefield : MonoBehaviour, IGraph
//     {
//         [SerializeField] private new Transform camera;
//         [SerializeField] private GameObject playerCharacterPrefab;
//         
//         
//         
//         public class Tile : IPositionNode
//         {
//             public HexCoord         m_coord;
//             public List<Link>       m_links = new List<Link>();
//
//             #region Properties
//
//             public Vector3 WorldPosition => m_coord.Position;
//
//             public IEnumerable<ILink> Links => m_links;
//
//             public Unit Unit { get; set; }
//
//             #endregion
//         }
//
//         [SerializeField, Range(0.01f, 0.1f)]
//         private float                       m_fLineThickness = 0.05f;
//
//         private Camera                      m_camera;
//
//         private Dictionary<HexCoord, Tile>  m_tiles = new Dictionary<HexCoord, Tile>();
//         private Tile                        m_hoverTile;
//
//         #region Properties
//
//         public IEnumerable<INode> Nodes => m_tiles.Values;
//
//         public Ray MouseRay => m_camera.ScreenPointToRay(Input.mousePosition);
//
//         public Tile HoverTile => m_hoverTile;
//
//         #endregion
//
//         private void OnEnable()
//         {
//             List<Vector3> vertices = new List<Vector3>();
//             List<int> triangles = new List<int>();
//
//             float fHalfSize = m_fLineThickness * 0.5f;
//             m_tiles.Clear();
//
//             // get all obstacles
//             HashSet<HexCoord> obstacles = new HashSet<HexCoord>();
//             foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
//             {
//                 HexCoord hc = HexCoord.GetHexCoordAt(obstacle.transform.position);
//                 obstacle.transform.position = hc.Position;
//                 obstacles.Add(hc);
//             }
//
//             foreach (HexCoord hc in HexCoord.RectangularGrid(11, 9))
//             {
//                 // blocked by obstacle?
//                 if (obstacles.Contains(hc))
//                 {
//                     continue;
//                 }
//
//                 Vector3[] corners = hc.Corners;
//
//                 // create tile mesh
//                 for (int i = 0; i < 6; ++i)
//                 {
//                     Vector3 vA = corners[i];
//                     Vector3 vB = corners[(i + 1) % 6];
//
//                     Vector3 vForward = Vector3.Normalize(vB - vA);
//                     Vector3 vRight = Vector3.Cross(vForward, Vector3.up);
//
//                     int iStart = vertices.Count;
//
//                     vertices.AddRange(new Vector3[] {
//                         vA + vRight * fHalfSize,
//                         vB + vRight * fHalfSize,
//                         vB - vRight * fHalfSize,
//                         vA - vRight * fHalfSize,
//                     });
//
//                     triangles.AddRange(new int[]
//                     {
//                         iStart + 0, iStart + 1, iStart + 2,
//                         iStart + 0, iStart + 2, iStart + 3,
//                     });
//                 }
//
//                 // create tile
//                 m_tiles[hc] = new Tile { m_coord = hc };
//             }
//
//             // create mesh
//             Mesh mesh = new Mesh();
//             mesh.name = "BattleFieldGridMesh";
//             mesh.hideFlags = HideFlags.DontSave;
//             mesh.vertices = vertices.ToArray();
//             mesh.colors = vertices.ConvertAll(v => Color.black).ToArray();
//             mesh.triangles = triangles.ToArray();
//             mesh.RecalculateBounds();
//             mesh.RecalculateNormals();
//
//             // assign mesh
//             GetComponent<MeshFilter>().mesh = mesh;
//
//             // create neighbor links
//             foreach (Tile tile in m_tiles.Values)
//             {
//                 foreach (HexCoord nc in tile.m_coord.Neighbors)
//                 {
//                     Tile neighbor;
//                     if (m_tiles.TryGetValue(nc, out neighbor))
//                     {
//                         tile.m_links.Add(new Link(tile, neighbor));
//                     }
//                 }
//             }
//         }
//
//         void Start()
//         {
//             m_camera = GetComponentInChildren<Camera>();
//         }
//
//         void Update()
//         {
//             // update hover tile
//             m_hoverTile = null;
//             Ray ray = MouseRay;
//             Plane ground = new Plane(Vector3.up, 0.0f);
//             float fEnter;
//             if (ground.Raycast(ray, out fEnter))
//             {
//                 Vector3 vHit = ray.origin + ray.direction * fEnter;
//                 HexCoord hc = HexCoord.GetHexCoordAt(vHit);
//                 m_tiles.TryGetValue(hc, out m_hoverTile);
//             }
//
//             // draw hover tile
//             if (m_hoverTile != null)
//             {
//                 HexUtils.DrawHex(m_hoverTile.m_coord, new Color(1.0f, 1.0f, 0.0f, 0.3f));
//             }
//         }
//     }
// }