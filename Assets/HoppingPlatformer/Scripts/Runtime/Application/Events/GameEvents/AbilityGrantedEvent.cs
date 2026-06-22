namespace HoppingPlatformer.Application.Events
{
    public readonly struct AbilityGrantedEvent : IGameEvent
    {
        public string AbilityName { get; }

        public AbilityGrantedEvent(string abilityName)
        {
            AbilityName = abilityName;
        }
    }
}