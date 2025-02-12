using UnityEngine;

namespace MeshHandlers
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class MeshHandler : MonoBehaviour
    { 
        protected MeshFilter MeshFilter { get; private set; }
        protected MeshRenderer MeshRenderer { get; private set; }

        private void OnEnable() => (MeshFilter, MeshRenderer) = (gameObject.GetComponent<MeshFilter>(), gameObject.GetComponent<MeshRenderer>());
        private void OnDisable() => (MeshFilter, MeshRenderer) = (null, null);
    }
}