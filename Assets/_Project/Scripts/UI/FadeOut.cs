using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LD44.UI
{
    public class FadeOut : MonoBehaviour
    {
        public Image Image;
        public float Duration = 1f;
        
        void Start()
        {
            StartCoroutine(FadeOutRoutine());
        }

        private IEnumerator FadeOutRoutine()
        {
            var current = 0f;
            var color = Image.color;

            while(current <= Duration)
            {
                current += Time.deltaTime;

                color.a = 1f - Mathf.Clamp01(current / Duration);
                Image.color = color;

                yield return null;
            }
            
            color.a = 0f;
            Image.color = color;
            yield return null;

            Destroy(this);
        }
    }
}