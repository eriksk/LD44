

using UnityEngine;

namespace LD44.UI
{
    [ExecuteInEditMode]
    public class UIFitToParent : MonoBehaviour
    {
        void Update()
        {
            var container = transform.parent as RectTransform;
            if(container == null) return;

            var t = transform as RectTransform;

            t.sizeDelta = container.sizeDelta;
            t.localPosition = Vector3.zero;
        }
    }
}