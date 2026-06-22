namespace HoppingPlatformer.Domain.Generation
{
    public interface IGenerationStep
    {
        void Execute(GenerationContext context);
    }
}