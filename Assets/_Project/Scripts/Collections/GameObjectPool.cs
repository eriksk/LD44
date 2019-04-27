using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD44.Collections.Pool
{
    // TODO: GC
    public class GameObjectPool<T> where T : Behaviour
    {
        public int Capacity { get; private set; }
        public List<T> ActiveItems => _active;
        public List<T> InactiveItems => _inactive;
        public int Active => _active.Count;
        public int Ramaining => _inactive.Count;

        public event Action<T> OnPushed;
        public event Action<T> OnPopped;

        private List<T> _active, _inactive;

        public GameObjectPool(GameObject prefab, int capacity)
        {
            Capacity = capacity;
            _active = new List<T>(capacity);
            _inactive = new List<T>(capacity);

            for(var i = 0; i < capacity; i++)
            {
                var item = GameObject.Instantiate(prefab);
                item.SetActive(false);
                _inactive.Add(item.GetComponent<T>());
            }
        }

        public void Clear()
        {
            while(_active.Count > 0)
            {
                Push(_active[0]);
            }
        }

        public T Pop()
        {
            if(_inactive.Count == 0) return null; // or throw exception?

            var item = _inactive[0];
            _inactive.RemoveAt(0);
            _active.Add(item);

            item.gameObject.SetActive(true);
            OnPopped?.Invoke(item);

            return item;
        }
        
        public void Push(T gameObject)
        {
            if(_active.Count == 0)
            {
                // or throw exception?
                return;
            }

            var index = _active.IndexOf(gameObject);
            
            if(index < 0) return; // or throw exception?

            var item = _active[index];
            item.gameObject.SetActive(false);
            _active.RemoveAt(index);
            _inactive.Add(item);
            OnPushed?.Invoke(item);
        }
    }
}