namespace HoppingPlatformer.Player
{
    public sealed class HexJumpService : IJumpService
    {
        private readonly LevelRuntime _level;

        public HexJumpService(LevelRuntime level)
        {
            _level = level;
        }

        public JumpResult ResolveJump(PlatformView current, JumpDirection direction, int distance)
        {
            HexPos targetPosition;

            HexPos currentPosition = current.Node.Position;
            HexGridTopology topology = _level.Topology;

            bool valid = (direction == JumpDirection.Left)
                ? topology.TryGetLeftJump(currentPosition, distance, out targetPosition)
                : topology.TryGetRightJump(currentPosition, distance, out targetPosition);

            if (!valid)
            {
                return new JumpResult(targetPosition);
            }

            if (!_level.TryGetPlatform(targetPosition, out PlatformView target))
            {
                return new JumpResult(targetPosition);
            }

            return new JumpResult(target);
        }
    }
}
