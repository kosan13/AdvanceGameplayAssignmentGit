using System.Collections.Generic;
using Game.General;
using UnityEngine;
namespace Game
{
    public class Level : ProceduralMesh
    {


        public static readonly Vector3Int[] Directions = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left, Vector3Int.up, Vector3Int.down };
        protected override Mesh CreateMesh()
        {
            // create mesh
            List<Vector3> vertices = new ();
            List<Vector2> uv = new ();
            List<Color> colors = new ();
            List<int> triangles = new ();
            AddQuad(new Vector3(1,1,1), Directions[0],  new Color(0.5f, 0.25f, 0.0f) , vertices, uv, colors, triangles);
            // create mesh
            Mesh mesh = new ();
            return mesh;
        }
        protected void AddQuad(Vector3 vPosition, Vector3 vDirection, Color c, List<Vector3> vertices, List<Vector2> uv, List<Color> colors, List<int> triangles)
        {
            Vector3 vRight = Vector3.Cross(vDirection, Vector3.up).normalized;

            // calculate verts
            int iStart = vertices.Count;
            vertices.AddRange(new []{
                vPosition + vDirection * 0.5f - vRight * 0.5f - Vector3.up * 0.5f,
                vPosition + vDirection * 0.5f - vRight * 0.5f + Vector3.up * 0.5f,
                vPosition + vDirection * 0.5f + vRight * 0.5f + Vector3.up * 0.5f,
                vPosition + vDirection * 0.5f + vRight * 0.5f - Vector3.up * 0.5f
            });

            // calculate uvs (planar mapping)
            for (int i = 0; i < 4; ++i)
            {
                Vector3 v = vertices[iStart + i];
                uv.Add(new Vector2(Vector3.Dot(vRight, v), Vector3.Dot(Vector3.up, v)));
            }

            // add colors
            colors.AddRange(new Color[] { c, c, c, c });

            // add triangles
            triangles.AddRange(new int[]{
                iStart + 0, iStart + 1, iStart + 2,
                iStart + 0, iStart + 2, iStart + 3
            });
        }
    }
}