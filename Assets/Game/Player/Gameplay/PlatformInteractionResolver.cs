namespace HoppingPlatformer.Player
{
    public sealed class PlatformInteractionResolver : IInteractionResolver
    {
        public void Resolve(PlatformView platform, PlayerControllerNew player)
        {
            var interaction = platform.GetComponentInChildren<IPlatformInteraction>();

            //interaction?.Interact(player);
        }
    }
}
