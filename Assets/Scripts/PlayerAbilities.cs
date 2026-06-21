public sealed class PlayerAbilities
{
    public int JumpDistance { get; private set; } = 1;

    public void GrantDoubleJump()
    {
        JumpDistance = 2;
    }

    public void Consume()
    {
        JumpDistance = 1;
    }
}