namespace HoppingPlatformer.Domain.Common
{
    public interface IRandom
    {
        int Range(int minInclusive, int maxExclusive);

        float Range(float min, float max);

        float Value();
    }
}