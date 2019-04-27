
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
        public MeshRenderer Renderer;

        private float _bulletDuration;
        private int _firedById;
        private float _lossWait;
        private Player _firedByPlayer;
        private float _timeSpentFree;
        private const float LossWaitDuration = 0.5f;

        public bool CanBePickedUp => _lossWait <= 0 && State == CoinState.Free;

        public Player FiredBy => _firedByPlayer;

        public void Reset()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            State = CoinState.Free;
            _firedById = -1;
            _bulletDuration = 0;
            _lossWait = 0f;
            _firedByPlayer = null;
            _timeSpentFree = 0f;
            Renderer.enabled = true;
        }

        public void Loss(Vector3 position, Vector3 target)
        {
            State = CoinState.Free;
            _lossWait = LossWaitDuration;
            _timeSpentFree = 0f;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            transform.position = position + Vector3.up * 1f;
            transform.rotation = Quaternion.identity;

            var directionToTarget = (target - position).normalized;

            var direction = new Vector3(
                UnityEngine.Random.Range(-1f, 1f) * 0.1f,
                0.5f,
                UnityEngine.Random.Range(-1f, 1f) * 0.1f
            ) + directionToTarget;

            var torque = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ) * 10f;

            Rigidbody.AddForce(direction * UnityEngine.Random.Range(5f, 10f), ForceMode.Impulse);
            Rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        public void Fire(Player player, Transform origin)
        {
            _firedByPlayer = player;
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
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if(!CanBePickedUp) return;
            if(!collider.gameObject.InMask(PlayerMask)) return;
            if(collider.gameObject.GetInstanceID() == _firedById) return;

            var player = collider.gameObject.GetComponent<Player>();
            if(player.Dead) return;

            player.PickUpCoin(this);
            ObjectLocator.Coins.Push(this);
        }

        void Update()
        {
            if(State == CoinState.Free) 
            {
                _timeSpentFree += Time.deltaTime;
                _lossWait -= Time.deltaTime;

                if(_timeSpentFree > 2f)
                {
                    Renderer.enabled = Mathf.Sin(Time.time * 40f) > 0f;
                }
                if(_timeSpentFree > 4f)
                {
                    ObjectLocator.Coins.Push(this);
                }
                return;
            }

            _bulletDuration -= Time.deltaTime;

            ObjectLocator.Particles.CoinTrail(transform.position, Rigidbody.velocity);

            if(_bulletDuration <= 1f || Rigidbody.velocity.magnitude < 0.1f)
            {
                _firedById = -1;
                _lossWait = LossWaitDuration;
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