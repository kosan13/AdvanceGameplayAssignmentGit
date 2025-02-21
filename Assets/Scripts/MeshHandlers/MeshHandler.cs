using System;
using UnityEngine;

namespace MeshHandlers
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class MeshHandler : MonoBehaviour
    { 
        protected MeshFilter MeshFilter { get; private set; }
        protected MeshRenderer MeshRenderer { get; private set; }

        private void OnValidate()
        {
            if (MeshFilter == null) MeshFilter = gameObject.GetComponent<MeshFilter>(); 
            if (MeshRenderer == null) MeshRenderer = gameObject.GetComponent<MeshRenderer>(); 
        }

        private void OnEnable() => (MeshFilter, MeshRenderer) = (gameObject.GetComponent<MeshFilter>(), gameObject.GetComponent<MeshRenderer>());
        private void OnDisable() => (MeshFilter, MeshRenderer) = (null, null);
    }
}