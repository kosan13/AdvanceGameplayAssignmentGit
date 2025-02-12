using System.Collections.Generic;
using Events;
using Graphs;
using UnityEngine;

namespace Game
{
    public class Battlescape : GameEventBehaviour, ISearchableGraph
    {
        private const int SIZE = 8;

        private Node[,] m_nodes;

        private void OnEnable()
        {
            EventHandler.Main.PushEvent(this);
        }

        #region Properties

        public IEnumerable<INode> GetNodes
        {
            get
            {
                if (m_nodes != null)
                    for (int z = 0; z < SIZE; z++)
                    for (int x = 0; x < SIZE; x++)
                        yield return m_nodes[x, z];
            }
        }

        #endregion

        public float Heuristic(INode start, INode goal)
        {
            if (start is Node A &&
                goal is Node B)
                return Vector3.Distance(A.m_vPosition, B.m_vPosition);

            return 1.0f;
        }

        public override void OnBegin(bool bFirstTime)
        {
            base.OnBegin(bFirstTime);
            GenerateGrid();
        }

        protected void GenerateGrid()
        {
            // generate a mesh
            List<Vector3> vertices = new();
            List<Color> colors = new();
            List<int> triangles = new();

            m_nodes = new Node[SIZE, SIZE];
            for (int z = 0; z < SIZE; z++)
            for (int x = 0; x < SIZE; x++)
            {
                int iStart = vertices.Count;
                vertices.AddRange(new Vector3[]
                {
                    new(x - 0.5f, 0.0f, z - 0.5f),
                    new(x - 0.5f, 0.0f, z + 0.5f),
                    new(x + 0.5f, 0.0f, z + 0.5f),
                    new(x + 0.5f, 0.0f, z - 0.5f)
                });

                Color c = (z + x) % 2 == 0 ? Color.white : Color.black;
                colors.AddRange(new[] { c, c, c, c });

                triangles.AddRange(new[]
                {
                    iStart + 0, iStart + 1, iStart + 2,
                    iStart + 0, iStart + 2, iStart + 3
                });

                m_nodes[x, z] = new Node { m_vPosition = new Vector3(x, 0.0f, z) };
            }

            // create links
            for (int z = 0; z < SIZE; z++)
            for (int x = 0; x < SIZE; x++)
            for (int z1 = -1; z1 <= 1; z1++)
            for (int x1 = -1; x1 <= 1; x1++)
            {
                Vector2Int v = new(x + x1, z + z1);
                if (v.x >= 0 && v.y >= 0 && v.x < SIZE && v.y < SIZE)
                {
                    Node A = m_nodes[x, z];
                    Node B = m_nodes[v.x, v.y];
                    if (A != B) A.m_links.Add(new Link(A, B,LinkDirection.Null));
                }
            }

            // create mesh
            Mesh mesh = new();
            mesh.vertices = vertices.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
        }

        public override bool IsDone()
        {
            return false; // return true when 1 team wins
        }

        public class Node : IPositionNode
        {
            public List<Link> m_links = new();
            public Vector3 m_vPosition;

            #region Properties

            public IEnumerable<ILink> GetLinks => m_links;

            public Vector3 GetWorldPosition => m_vPosition;

            #endregion
        }
    }
}