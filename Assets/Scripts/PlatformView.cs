using UnityEngine;

public sealed class PlatformView : MonoBehaviour
{
    public PlatformNode Node { get; private set; }

    public void Initialize(
        PlatformNode node)
    {
        Node = node;
    }
}
