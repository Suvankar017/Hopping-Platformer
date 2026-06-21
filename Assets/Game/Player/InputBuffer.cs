using UnityEngine;

namespace HoppingPlatformer.Player
{
    public sealed class InputBuffer<T>
    {
        private readonly float _duration;

        private bool _hasValue;

        private T _value;

        private float _timestamp;

        public bool HasBufferedInput
        {
            get
            {
                if (!_hasValue)
                    return false;

                return Time.time - _timestamp <= _duration;
            }
        }

        public InputBuffer(float duration)
        {
            _duration = duration;
        }

        public void Store(T value, float currentTime)
        {
            _hasValue = true;

            _value = value;

            _timestamp = currentTime;
        }

        public bool TryConsume(float currentTime, out T value)
        {
            value = default;

            if (!_hasValue)
                return false;

            if (currentTime - _timestamp > _duration)
            {
                _hasValue = false;

                return false;
            }

            value = _value;

            _hasValue = false;

            return true;
        }

        public void Clear()
        {
            _hasValue = false;
        }
    }
}
