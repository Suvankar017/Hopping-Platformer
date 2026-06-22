using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoppingPlatformer.Player
{
    public readonly struct JumpResult
    {
        public readonly bool Success;

        public readonly PlatformView Platform;

        public readonly HexPos FailedPosition;

        public JumpResult(PlatformView platform)
        {
            Success = true;

            Platform = platform;

            FailedPosition = default;
        }

        public JumpResult(HexPos failedPosition)
        {
            Success = false;

            Platform = null;

            FailedPosition = failedPosition;
        }
    }
}
