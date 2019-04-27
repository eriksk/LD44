using LD44.Effects;
using LD44.Game.Coins;
using UnityEngine;

namespace LD44.Game
{
    public static class ObjectLocator
    {
        private static InstancedParticleSystem _particles;
        private static CoinManager _coinManager;

        public static void Clear()
        {
            _particles = null;
            _coinManager = null;
        }
        
        private static T GetOrFind<T>(T existing, string name, string containerName = "") where T : Behaviour
        {
            if(existing != null) return existing;

            var gameObject = GameObject.Find(name);
            
            if(gameObject == null) // find if inactive
            {
                gameObject = GameObject.Find(containerName).transform.Find(name)?.gameObject;
            }

            if(gameObject == null)
            {
                Debug.LogError($"Could not find '{typeof(T).FullName}' with name '{name}'");
                return null;
            }

            var component = gameObject.GetComponent<T>();

            if(component == null)
            {
                Debug.LogError($"Could not find component '{typeof(T).FullName}' for game object, accessor name '{name}'", gameObject);
            }

            return component;
        }
    
        public static InstancedParticleSystem Particles => _particles = GetOrFind(_particles, "[Particles]");
        public static CoinManager Coins => _coinManager = GetOrFind(_coinManager, "[CoinManager]");
    }
}