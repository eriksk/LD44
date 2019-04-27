using LD44.Collections.Pool;
using UnityEngine;

namespace LD44.Effects
{
    public class DestructablePiggyManager : MonoBehaviour
    {
        public GameObject DestructablePiggyPrefab;
        public int Capacity = 32;

        private GameObjectPool<DestructablePiggy> _pool;

        void Start()
        {
            _pool = new GameObjectPool<DestructablePiggy>(DestructablePiggyPrefab, Capacity);
            foreach(var item in _pool.InactiveItems)
            {
                item.Initiate();
            }
        }

        public void Execute(Transform location, Material material)
        {
            var piggy = _pool.Pop();
            piggy.Reset();
            piggy.transform.position = location.position;
            piggy.transform.rotation = location.rotation;
            piggy.Explode(material);
        }

        void Update()
        {
            for(var i = 0; i < _pool.ActiveItems.Count; i++)
            {
                var item = _pool.ActiveItems[i];
                if(item.Dead)
                {
                    _pool.Push(item);
                    i--;
                }
            }
        }
    }
}