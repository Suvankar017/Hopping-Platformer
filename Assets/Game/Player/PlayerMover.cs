using System;
using System.Collections;
using UnityEngine;

namespace HoppingPlatformer.Player
{
    public sealed class PlayerMover : IPlayerMover
    {
        private readonly MonoBehaviour _runner;

        private readonly PlayerMovementSettings _settings;

        public bool IsMoving { get; private set; }

        public PlayerMover(MonoBehaviour runner, PlayerMovementSettings settings)
        {
            _runner = runner;
            _settings = settings;
        }

        public void JumpTo(Transform actor, Vector3 targetPosition, Action onComplete)
        {
            _runner.StartCoroutine(JumpRoutine(actor, targetPosition, onComplete));
        }

        public void FallTo(Transform actor, Vector3 targetPosition, Action onComplete)
        {
            _runner.StartCoroutine(FallRoutine(actor, targetPosition, onComplete));
        }

        private IEnumerator JumpRoutine(Transform actor, Vector3 target, Action onComplete)
        {
            IsMoving = true;

            Vector3 start = actor.position;

            float elapsed = 0.0f;

            while (elapsed < _settings.JumpDuration)
            {
                elapsed += Time.deltaTime;

                float t = elapsed / _settings.JumpDuration;

                Vector3 pos = Vector3.Lerp(start, target, t);

                pos.y += Mathf.Sin(t * Mathf.PI) * _settings.JumpHeight;

                actor.position = pos;

                yield return null;
            }

            actor.position = target;

            IsMoving = false;

            onComplete?.Invoke();
        }

        private IEnumerator FallRoutine(Transform actor, Vector3 target, Action onComplete)
        {
            IsMoving = true;

            Vector3 start = actor.position;

            float elapsed = 0.0f;

            while (elapsed < _settings.JumpDuration)
            {
                elapsed += Time.deltaTime;

                float t = elapsed / _settings.JumpDuration;

                Vector3 pos = Vector3.Lerp(start, target, t);

                pos.y += Mathf.Sin(t * Mathf.PI) * _settings.JumpHeight;

                actor.position = pos;

                yield return null;
            }

            while (actor.position.y > target.y - 2.0f)
            {
                actor.position += _settings.FallSpeed * Time.deltaTime * Vector3.down;

                yield return null;
            }

            IsMoving = false;

            onComplete?.Invoke();
        }
    }
}
