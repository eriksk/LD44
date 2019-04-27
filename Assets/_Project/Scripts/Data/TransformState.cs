using UnityEngine;

namespace LD44.Data
{
    public struct TransformState
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;

        public static TransformState Create(Transform transform)
        {
            return new TransformState()
            {
                LocalPosition = transform.localPosition,
                LocalRotation = transform.localRotation
            };
        }

        public void Restore(Transform transform)
        {
            transform.localPosition = LocalPosition;
            transform.localRotation = LocalRotation;
        }
    }
}