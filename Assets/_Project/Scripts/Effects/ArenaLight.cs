
using UnityEngine;

namespace LD44.Effects
{
    public class ArenaLight : MonoBehaviour
    {
        public Material Material;
        public float ChangeInterval = 1f;

        private float _current;
        private Vector2 _offset;


        void Update()
        {
            _current += Time.deltaTime;

            if(_current >= ChangeInterval)
            {
                _current = 0f;
                _offset.x += 1f / 8f;
            }

            Material.SetTextureOffset("_MainTex", _offset);
            Material.SetTextureOffset("_EmissionMap", _offset);
        }
    }
}