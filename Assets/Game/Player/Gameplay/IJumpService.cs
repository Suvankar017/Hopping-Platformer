namespace HoppingPlatformer.Player
{
    public interface IJumpService
    {
        JumpResult ResolveJump(PlatformView current, JumpDirection direction, int distance);
    }
}
