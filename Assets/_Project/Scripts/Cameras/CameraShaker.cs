
using UnityEngine;

namespace LD44.Cameras
{
    public class CameraShaker : MonoBehaviour
    {
        public Transform Camera;
        public float Intensity = 1f;
        [Range(0.1f, 1f)]
        public float Duration = 1f;

        private Vector3 _offset;
        private float _amount;
        private float _current;

        public void Start()
        {
            _current = Duration;
        }

        public void Shake()
        {
            _current = 0f;
        }

        void Update()
        {
            _current += Time.deltaTime;
            _amount = 1f - Mathf.Clamp01(_current / Duration);

            if(_amount <= 0)
            {
                Camera.localPosition = Vector3.zero;
                return;
            }

            _offset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            );

            Camera.localPosition = _offset * _amount * Intensity;
        }
    }
}