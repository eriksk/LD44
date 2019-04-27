
using System.Collections.Generic;
using LD44.Collections.Pool;
using UnityEngine;

namespace LD44.Game.Coins
{
    public class CoinManager : MonoBehaviour
    {
        public GameObject CoinPrefab;
        public int Capacity = 1023;

        private GameObjectPool<Coin> _coinPool;

        void Start()
        {
            _coinPool = new GameObjectPool<Coin>(CoinPrefab, Capacity);
        }

        public List<Coin> Active => _coinPool.ActiveItems;

        public Coin Pop()
        {
            var coin = _coinPool.Pop();
            coin.Reset();
            return coin;
        }

        public void Push(Coin coin)
        {
            _coinPool.Push(coin);
        }
    }
}