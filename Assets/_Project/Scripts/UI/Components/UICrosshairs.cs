
using UnityEngine;

namespace LD44.UI.Components
{
    public class UICrosshairs : MonoBehaviour
    {
        public LayerMask GroundMask;

        public Vector3 GetDirection(Vector3 origin)
        {
            var groundPosition = GetGroundPosition(origin);
            groundPosition.y = 0f;
            origin.y = 0f;

            return (groundPosition - origin).normalized;
        }

        public void Update()
        {
            transform.position = Input.mousePosition;
        }

        private Vector3 GetGroundPosition(Vector3 defaultValue)
        {
            var ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, GroundMask, QueryTriggerInteraction.Ignore))
            {
                return hit.point;
            }

            return defaultValue;
        }

        void OnDrawGizmos()
        {
            var position = GetGroundPosition(Vector3.zero);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, 1);
        }
    }
}