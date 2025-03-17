using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Librarys.MeshHandlers.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public abstract class MeshHandler  : MonoBehaviour
    {
        #region Properties
        /// <summary>
        ///   <para>A variable to access the Mesh of the object.</para>
        /// </summary>
        public MeshFilter MeshFilter { get; private set; } 
        /// <summary>
        ///   <para>A variable to access the MeshRenderer of the object.</para>
        /// </summary>
        public MeshRenderer MeshRenderer { get; private set; }
        /// <summary>
        ///   <para>A variable to access the MeshCollider of the object.</para>
        /// </summary>
        public MeshCollider MeshCollider { get; private set; }
        
        /// <summary>
        ///   <para>A function to set the Mesh of the object.</para>
        /// </summary>
        public MeshFilter SetMeshFilter(MeshFilter meshFilter) => MeshFilter = meshFilter;
        /// <summary>
        ///   <para>A function to set the MeshRenderer of the object.</para>
        /// </summary>
        public MeshRenderer SetMeshRenderer(MeshRenderer meshRenderer) => MeshRenderer = meshRenderer;
        /// <summary>
        ///   <para>A function to set the MeshCollider of the object.</para>
        /// </summary>
        public MeshCollider SetMeshFilter(MeshCollider meshCollider) => MeshCollider = meshCollider;

        #endregion
        protected virtual void OnValidate()
        {
            if (MeshFilter == null) MeshFilter = gameObject.GetComponent<MeshFilter>(); 
            if (MeshRenderer == null) MeshRenderer = gameObject.GetComponent<MeshRenderer>(); 
            if (MeshCollider == null) MeshCollider = gameObject.GetComponent<MeshCollider>(); 
        }
        protected virtual void OnEnable()
        {
            MeshFilter = gameObject.GetComponent<MeshFilter>();
            MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            MeshCollider = gameObject.GetComponent<MeshCollider>();
        }
        protected virtual void OnDisable() => (MeshFilter, MeshRenderer, MeshCollider) = (null, null, null);

        
        /// <summary>
        ///   <para>Creat a new Mesh</para>
        ///   <param name="combineInstance">The new Mesh</param>
        /// </summary>
        public static Mesh NewMesh(CombineInstance combineInstance) => RecalculateAndOptimizeMesh(combineInstance.mesh);
        
        /// <summary>
        ///   <para>Creat a new Mesh</para>
        ///   <param name="combineInstances">All the sub new SubMeshes</param>
        ///   <param name="mergeSubMeshes">Defines whether Meshes should be combined into a single sub-mesh.</param>
        ///   <param name="useMatrices">Defines whether the transforms supplied in the CombineInstance array should be used or ignored.</param>
        /// </summary>
        public static Mesh NewMesh(List<CombineInstance> combineInstances, bool mergeSubMeshes = false, bool useMatrices = false)
        {
            Mesh mesh = new () { indexFormat = IndexFormat.UInt32, };
            mesh.CombineMeshes(combineInstances.ToArray(), mergeSubMeshes, useMatrices);
            return RecalculateAndOptimizeMesh(mesh);
        }
        
        /// <summary>
        ///   <para>Creat a new Mesh from values</para>
        /// </summary>
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
            return RecalculateAndOptimizeMesh(mesh);
        }
        
        /// <summary>
        ///   <para>Recalculate Bounds and Normals fo the Mesh and Optimize the Mesh </para>
        /// </summary>
        public static Mesh RecalculateAndOptimizeMesh(Mesh mesh)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();
            return mesh;
        }
    }
}