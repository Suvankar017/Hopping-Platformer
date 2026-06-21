using UnityEngine;

public sealed class CoinInteraction
    : MonoBehaviour,
      IPlatformInteraction
{
    public void Interact(
        PlayerController player)
    {
        player.AddCoin();

        Destroy(gameObject);
    }
}