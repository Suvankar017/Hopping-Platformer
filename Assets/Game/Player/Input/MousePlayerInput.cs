using System;
using UnityEngine;

namespace HoppingPlatformer.Player
{
    public sealed class MousePlayerInput : MonoBehaviour, IPlayerInput
    {
        [SerializeField]
        private Camera _targetCamera;

        private Transform _target;

        public event Action<JumpDirection> JumpRequested;

        private void Awake()
        {
            if (_targetCamera == null)
                _targetCamera = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            Vector3 mouse = _targetCamera.ScreenToWorldPoint(Input.mousePosition);

            JumpDirection direction =
                (mouse.x < _target.position.x)
                ? JumpDirection.Left
                : JumpDirection.Right;

            JumpRequested?.Invoke(direction);
        }

        public void Init(Transform target)
        {
            _target = target;
        }
    }
}
