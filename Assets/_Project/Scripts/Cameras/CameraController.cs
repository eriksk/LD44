
using LD44.Game;
using UnityEngine;

namespace LD44.Cameras
{
    public class CameraController : MonoBehaviour
    {
        public float MaxRange = 3f;
        public float Speed = 3f;

        private Vector3 _initialPosition;

        void Start()
        {
            _initialPosition = transform.position;
        }

        void Update()
        {
            var player = ObjectLocator.GameManager.Player;
            var targetOffset = Vector3.zero;

            if(player != null && !player.Dead)
            {
                targetOffset = new Vector3(
                    Mathf.Clamp01(Mathf.Abs(player.transform.position.x) / MaxRange) * Mathf.Sign(player.transform.position.x) * MaxRange,
                    0f,
                    Mathf.Clamp01(Mathf.Abs(player.transform.position.z) / MaxRange) * Mathf.Sign(player.transform.position.z) * MaxRange
                );
            }

            transform.position = Vector3.Lerp(
                transform.position,
                _initialPosition + targetOffset,
                Speed * Time.deltaTime
            );
        }
    }
}