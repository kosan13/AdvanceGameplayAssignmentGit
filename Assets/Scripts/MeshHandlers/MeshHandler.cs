using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshHandlers
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public abstract class MeshHandler : MonoBehaviour
    { 
        public MeshFilter MeshFilter { get; private set; }
        public MeshRenderer MeshRenderer { get; private set; }
        public MeshCollider MeshCollider { get; private set; }

        private void OnValidate()
        {
            if (MeshFilter == null) MeshFilter = gameObject.GetComponent<MeshFilter>(); 
            if (MeshRenderer == null) MeshRenderer = gameObject.GetComponent<MeshRenderer>(); 
            if (MeshCollider == null) MeshCollider = gameObject.GetComponent<MeshCollider>(); 
        }

        private void OnEnable() => (MeshFilter, MeshRenderer, MeshCollider) = (gameObject.GetComponent<MeshFilter>(), gameObject.GetComponent<MeshRenderer>(), gameObject.GetComponent<MeshCollider>());
        private void OnDisable() => (MeshFilter, MeshRenderer, MeshCollider) = (null, null, null);

        public static Mesh NewMesh(List<Vector3> vertices, List<Vector2> uv, List<Color> colors, List<int> triangles)
        {
            Mesh mesh = new ()
            {
                indexFormat = IndexFormat.UInt32,
                vertices = vertices.ToArray(), 
                uv = uv.ToArray(), 
                colors = colors.ToArray(), 
                triangles = triangles.ToArray()
            };
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();
            return mesh;
        }
    }
}