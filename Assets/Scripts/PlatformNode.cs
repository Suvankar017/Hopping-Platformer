public sealed class PlatformNode
{
    public HexPos Position;

    public PlatformType Type;

    public ItemType Item;

    public bool CanReachGoal;

    public HexPos NextTowardsGoal;

    public PlatformNode(
        HexPos position,
        PlatformType type = PlatformType.Normal)
    {
        Position = position;
        Type = type;
        Item = ItemType.None;
    }
}
