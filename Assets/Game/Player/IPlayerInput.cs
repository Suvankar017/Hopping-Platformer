using System;

namespace HoppingPlatformer.Player
{
    public interface IPlayerInput
    {
        event Action<JumpDirection> JumpRequested;
    }
}
