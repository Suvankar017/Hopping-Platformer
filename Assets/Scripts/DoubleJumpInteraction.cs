using UnityEngine;

public sealed class DoubleJumpInteraction
    : MonoBehaviour,
      IPlatformInteraction
{
    public void Interact(
        PlayerController player)
    {
        player.GrantDoubleJump();

        Destroy(gameObject);
    }
}