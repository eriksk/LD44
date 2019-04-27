
using UnityEngine;

namespace LD44.UI.Components
{
    public class UICrosshairs : MonoBehaviour
    {
        public Vector3 GetDirection(Vector3 origin)
        {
            return Vector3.forward;
        }

        public void Update()
        {
            transform.position = Input.mousePosition;
        }
    }

}