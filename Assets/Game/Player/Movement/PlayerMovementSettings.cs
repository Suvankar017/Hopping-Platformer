using UnityEngine;

namespace HoppingPlatformer.Player
{
    [System.Serializable]
    public sealed class PlayerMovementSettings
    {
        [SerializeField]
        private float _jumpDuration = 0.25f;

        [SerializeField]
        private float _jumpHeight = 1.5f;

        [SerializeField]
        private float _fallSpeed = 10.0f;


        public float JumpDuration => _jumpDuration;

        public float JumpHeight => _jumpHeight;

        public float FallSpeed => _fallSpeed;
    }
}
