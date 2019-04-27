using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD44.Textures
{
    [CreateAssetMenu(menuName="Custom/Textures/Procedural Texture")]
    public class ProceduralTexture : ScriptableObject
    {
        public FilterMode FilterMode = FilterMode.Point;
        public List<Gradient> Gradients;
        [Range(1, 256)]
        public int Resolution = 16;

        public string TextureName { get { return "_texture"; } }
        
        public Color[] GetPixels(int width, int height)
        {
            var colors = new List<Color>();

            for(var x = 0; x < width; x++)
            {
                var progress = x / (float)width;
                for(var y = 0; y < height; y++)
                {
                    var index = (int)((y / (float)height) * Gradients.Count);

                    var color = Gradients[Mathf.Clamp(index, 0, Gradients.Count - 1)].Evaluate(progress);
                    colors.Add(color);
                }
            }

            return colors.ToArray();
        }
    }
}