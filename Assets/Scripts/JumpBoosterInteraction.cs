using UnityEngine;

public sealed class JumpBoosterInteraction :
    MonoBehaviour,
    IPlatformInteraction
{
    [SerializeField]
    private int skipFloors = 4;

    public void Interact(
        PlayerController player)
    {
        player.PerformSkipJump(
            skipFloors);

        Destroy(gameObject);
    }
}