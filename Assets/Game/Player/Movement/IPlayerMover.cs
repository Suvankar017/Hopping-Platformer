using System;
using UnityEngine;

namespace HoppingPlatformer.Player
{
    public interface IPlayerMover
    {
        bool IsMoving { get; }

        void JumpTo(Transform actor, Vector3 target, Action onComplete);

        void FallTo(Transform actor, Vector3 target, Action onComplete);
    }
}
