using System;
using UnityEngine;

namespace HoppingPlatformer.Player
{
    public sealed class KeyboardPlayerInput : MonoBehaviour, IPlayerInput
    {
        public event Action<JumpDirection> JumpRequested;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                JumpRequested?.Invoke(JumpDirection.Left);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                JumpRequested?.Invoke(JumpDirection.Right);
            }
        }
    }
}
