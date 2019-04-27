
using System;
using LD44.Collections.Pool;
using LD44.Game.Players;
using UnityEngine;

namespace LD44.Game.Coins
{
    public class Coin : MonoBehaviour, IPoolable
    {
        public CoinState State;
        public Rigidbody Rigidbody;

        private float _bulletDuration;

        public void Reset()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            State = CoinState.Free;
        }

        public void Fire(Player player, Transform origin)
        {
            State = CoinState.Bullet;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            transform.position = origin.position;
            transform.rotation = origin.rotation * Quaternion.Euler(90f, 0f, 0f);

            Rigidbody.AddForce(origin.forward * 30f, ForceMode.Impulse);
            _bulletDuration = 2f;
        }

        void Update()
        {
            if(State == CoinState.Free) return;

            _bulletDuration -= Time.deltaTime;

            ObjectLocator.Particles.CoinTrail(transform.position, Rigidbody.velocity);

            if(_bulletDuration <= 0f || Rigidbody.velocity.magnitude < 0.5f)
            {
                State = CoinState.Free;
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
        }
        
        void FixedUpdate()
        {
            if(State == CoinState.Free) return;
            
        }
    }
}