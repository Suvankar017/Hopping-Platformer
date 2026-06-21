using System.Collections;
using UnityEngine;

public sealed class CameraController
    : MonoBehaviour
{
    [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField]
    private float upwardLookAhead = 3f;

    private Transform _target;

    private float _velocityY;

    private float _highestY;

    public void Initialize(
    Transform target)
    {
        _target = target;

        _highestY =
            target.position.y;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        float playerY =
            _target.position.y;

        _highestY =
            Mathf.Max(
                _highestY,
                playerY);

        float desiredY =
            _highestY +
            upwardLookAhead;

        Vector3 position =
            transform.position;

        position.y =
            Mathf.SmoothDamp(
                position.y,
                desiredY,
                ref _velocityY,
                smoothTime);

        transform.position =
            position;
    }

    public void EnterFalling()
    {
    }

    public void EnterVictory()
    {
        enabled = false;

        StartCoroutine(VictoryPanRoutine());
    }

    private IEnumerator VictoryPanRoutine()
    {
        Vector3 start =
            transform.position;

        Vector3 end =
            start +
            Vector3.up * 4f;

        float duration = 2f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / duration;

            transform.position =
                Vector3.Lerp(
                    start,
                    end,
                    t);

            yield return null;
        }
    }
}