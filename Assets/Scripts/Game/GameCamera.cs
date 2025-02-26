using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        #region Properties
        public Camera Camera { get; private set; }
        public static GameCamera Instance { get; private set; }
        
        #endregion
        private void OnEnable()
        {
            Instance = this;
            Camera = gameObject.GetComponent<Camera>();
        }
        private void OnDisable() => Instance = Instance == this ? null : Instance;
    }
}