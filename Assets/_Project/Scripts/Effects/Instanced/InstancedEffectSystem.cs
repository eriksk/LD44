using LD44.Collections.Pool;
using UnityEngine;
using UnityEngine.Rendering;

namespace LD44.Effects.Instanced
{
    public abstract class InstancedEffectSystem<T> : MonoBehaviour where T : InstancedEntity, new()
    {
        public int Capacity = 1023;
        public Mesh Mesh;
        public Material Material;
        public bool ReceiveShadows = true;
        public ShadowCastingMode ShadowCastingMode = ShadowCastingMode.On;
        public int AliveCount;

        protected Pool<T> Items;
        private Matrix4x4[] _renderMatrices;

        public virtual void Start()
        {
            Items = new Pool<T>(Capacity);
            _renderMatrices = new Matrix4x4[1023];
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        protected virtual void Update()
        {
            AliveCount = Items.Count;
            Render();
        }

        protected virtual void Render()
        {
            if(Mesh == null) return;
            if(Material == null) return;
            if(!Material.enableInstancing) return;

            var chunks = (Items.Count / 1023) + 1;

            for(var chunk = 0; chunk < chunks; chunk++)
            {
                var count = 0;

                for(var i = chunk * 1023; i < Items.Count; i++)
                {
                    var item = Items[i];

                    _renderMatrices[count++] = item.Matrix;

                    if(count > 1022)
                    {
                        break;
                    }
                }

                Graphics.DrawMeshInstanced(
                    Mesh, 
                    0, 
                    Material, 
                    _renderMatrices, 
                    count,
                    null, 
                    ShadowCastingMode, 
                    ReceiveShadows,
                    gameObject.layer);
            }
        }
    }
}