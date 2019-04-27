using System.Collections.Generic;

namespace LD44.Collections.Pool
{
    public class Pool<T> where T : class, new()
    {
        private List<T> _items;
        private int _count;
        private readonly int _capacity;

        public Pool(int capacity)
        {
            _capacity = capacity;
            _items = new List<T>(capacity);
            _count = 0;
            for(var i = 0; i < _capacity; i++)
            {
                _items.Add(new T());
            }
        }

        public int Count => _count;

        public void Clear()
        {
            _count = 0;
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        public T Pop()
        {
            if(_count > _capacity - 1)
            {
                return null;
            }

            return _items[_count++];
        }

        public void Push(int index)
        {
            if(_count == 0) return;
            if(index > _count - 1) return;

            var temp = _items[_count - 1];
            _items[_count - 1] = _items[index];
            _items[index] = temp;
            _count--;
        }

        public void Push(T item)
        {
            var index = _items.IndexOf(item);
            Push(index);
        }
    }

    public interface IPoolable
    {
        void Reset();
    }
}