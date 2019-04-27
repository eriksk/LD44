
using System;
using LD44.Collections.Pool;
using LD44.Extensions;
using LD44.Game.Players;
using UnityEngine;

namespace LD44.Game.Coins
{
    public class Coin : MonoBehaviour, IPoolable
    {
        public CoinState State;
        public Rigidbody Rigidbody;
        public LayerMask PlayerMask;

        private float _bulletDuration;
        private int _firedById;

        public void Reset()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            State = CoinState.Free;
            _firedById = -1;
        }

        public void Loss(Vector3 position)
        {
            State = CoinState.Free;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            transform.position = position + Vector3.up * 1f;
            transform.rotation = Quaternion.identity;

            var direction = new Vector3(
                UnityEngine.Random.Range(-1f, 1f) * 0.5f,
                1f,
                UnityEngine.Random.Range(-1f, 1f) * 0.5f
            );

            var torque = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ) * 10f;

            Rigidbody.AddForce(direction * UnityEngine.Random.Range(10f, 20f), ForceMode.Impulse);
            Rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        public void Fire(Player player, Transform origin)
        {
            _firedById = player.gameObject.GetInstanceID();
            State = CoinState.Bullet;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            transform.position = origin.position;
            transform.rotation = origin.rotation * Quaternion.Euler(90f, 0f, 0f);

            Rigidbody.AddForce(origin.forward * 30f, ForceMode.Impulse);
            _bulletDuration = 2f;
        }

        void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.InMask(PlayerMask)) return;
            if(collision.gameObject.GetInstanceID() == _firedById) return;

            if(State == CoinState.Bullet)
            {
                var player = collision.gameObject.GetComponent<Player>();
                player.OnHitByCoin(this);

                ObjectLocator.Particles.Explosion(transform.position, Rigidbody.velocity);
                ObjectLocator.Coins.Push(this);
                return;
            }
        }

        void Update()
        {
            if(State == CoinState.Free) return;

            _bulletDuration -= Time.deltaTime;

            ObjectLocator.Particles.CoinTrail(transform.position, Rigidbody.velocity);

            if(_bulletDuration <= 0f || Rigidbody.velocity.magnitude < 0.1f)
            {
                State = CoinState.Free;
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
        }
        
        void FixedUpdate()
        {
            if(State == CoinState.Free)
            {
                // Extra gravity
                Rigidbody.AddForce(Vector3.down * 20f);
            }

            if(State == CoinState.Bullet)
            {
                // Negate gravity
                Rigidbody.AddForce(-Physics.gravity * Rigidbody.mass);
            }
        }
    }
}