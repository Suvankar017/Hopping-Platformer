using System;

namespace HoppingPlatformer.Player
{
    public sealed class CompositePlayerInput
        : IPlayerInput
    {
        public event Action<JumpDirection> JumpRequested;

        public CompositePlayerInput(params IPlayerInput[] inputs)
        {
            foreach (var input in inputs)
            {
                input.JumpRequested += RaiseJump;
            }
        }

        private void RaiseJump(JumpDirection direction)
        {
            JumpRequested?.Invoke(direction);
        }
    }
}
