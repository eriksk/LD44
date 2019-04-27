using System;
using UnityEditor;
using UnityEngine;
using LD44.Textures;

namespace LD44
{
    [CustomEditor(typeof(Textures.ProceduralTexture))]
    public class ProceduralTextureEditor : Editor
    {
        private Texture2D _previewTexture;
        private bool _liveUpdate = false;

        public Textures.ProceduralTexture Target { get { return target as Textures.ProceduralTexture; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(Target.Gradients != null && Target.Gradients.Count > 0)
            {
                _liveUpdate = GUILayout.Toggle(_liveUpdate, "Live Update");

                if(GUILayout.Button("Refresh") || _liveUpdate)
                {
                    Generate();
                }

                UpdatePreviewTexture();
                EditorGUI.DrawPreviewTexture(new Rect(16, 256 + (Target.Gradients.Count * 12f), 256, 256), _previewTexture);
            }
            else
            {
                GUILayout.Label("Invalid texture");
            }
        }

        private void UpdatePreviewTexture()
        {
            _previewTexture = new Texture2D(Target.Resolution, Target.Resolution);
            _previewTexture.filterMode = Target.FilterMode;
            _previewTexture.SetPixels(Target.GetPixels(Target.Resolution, Target.Resolution));
            _previewTexture.Apply(true);
        }

        public void Generate()
        {
            if(Target.Gradients == null) return;
            if(Target.Gradients.Count == 0) return;


            var folder = AssetDatabase.GetAssetPath(Target).Replace(".asset", "");
            var texturePath = folder + Target.TextureName + ".asset";

            var texture = GetOrCreateTexture(texturePath);

            texture.filterMode = Target.FilterMode;
            texture.SetPixels(Target.GetPixels(Target.Resolution, Target.Resolution));
            texture.Apply(true);

            if(string.IsNullOrEmpty(AssetDatabase.GetAssetPath(texture)))
            {
                AssetDatabase.CreateAsset(texture, texturePath);
            }
        }

        private Texture2D GetOrCreateTexture(string path)
        {
            Texture2D texture = null;
            try
            {
                texture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                if(texture != null)
                {
                    texture.Resize(Target.Resolution, Target.Resolution);
                }
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }

            if(texture == null)
                texture = new Texture2D(Target.Resolution, Target.Resolution, TextureFormat.ARGB32, true);
            
            return texture;
        }
    }
}