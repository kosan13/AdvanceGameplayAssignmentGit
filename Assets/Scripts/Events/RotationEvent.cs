using Game;
using Game.UnitClasses;
using UnityEngine;

namespace Events
{
    public class RotationEvent : GameEventBehaviour
    {
        private const float CameraSensitivity = 5f;
        private const int CameraClampMin = -30;
        private const int CameraClampMax = 30;
        
        private bool _done;
        private float _rotationX;
        private readonly Camera _camera = GameCamera.Instance.Camera;

        private void OnEnable() => EventHandler.Main.PushEvent(this);
        private void OnDestroy() => _done = true;
        private void Update()
        {
            if (!Input.GetMouseButton(1)) return;
            float deltaY = Input.GetAxis("Mouse X") * CameraSensitivity;
            PlayerCharacter.Instance.transform.rotation *= Quaternion.Euler(0, deltaY, 0 );

            _rotationX += Input.GetAxis("Mouse Y") * CameraSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, CameraClampMin, CameraClampMax);
            _camera.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        }
        public override bool IsDone() => _done;
        public static void CreatRotationEvent() => new GameObject("RotationEvent").AddComponent<RotationEvent>();
    }
}