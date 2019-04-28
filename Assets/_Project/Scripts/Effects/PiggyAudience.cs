
using UnityEngine;

namespace LD44.Effects
{
    public class PiggyAudience : MonoBehaviour
    {
        public Material[] Materials;

        void Start()
        {
            for(var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                var renderer = child.GetComponentInChildren<Renderer>();
                renderer.sharedMaterial = Materials[UnityEngine.Random.Range(0, Materials.Length)];
                var animator = child.GetComponent<Animation>();
                // animator.
            }
        }
    }
}