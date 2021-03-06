
using System;
using LD44.Game.Coins;
using UnityEngine;

namespace LD44.Game.Players
{
    public class Player : MonoBehaviour
    {
        public float Speed = 1f;
        public float MovementDamping = 3f;
        public float RotationSpeed = 5f;

        public PlayerInput Input;
        public AudioSource Audio;
        public AudioSource FootStepsAudio;
        public AudioClip PickupCoinClip, ShootClip, HitClip;

        public Animation Animator;
        public Transform Muzzle;
        public int Budget = 150;

        public AIParameters AIParameters = new AIParameters();

        private string _currentAnimation;

        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Vector3 _aimDirection;
        private bool _fire;

        public bool Dead => Budget <= 0;
        private bool _isHumanPlayer;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetAnimation("idle", 0f);
            Input.OnStart(this);
            _isHumanPlayer = Input.GetType() == typeof(HumanPlayerInput);

            if(_isHumanPlayer)
            {
                FootStepsAudio.Play();
            }
            else
            {
                FootStepsAudio.Stop();
            }
        }

        private void SetAnimation(string name, float fadeLength = 0.1f)
        {
            if(_currentAnimation == name) return;
            _currentAnimation = name;

            Animator.CrossFade(_currentAnimation, fadeLength);
        }

        public void UpdateInput(Vector3 movement, Vector3 aimDirection, bool fire)
        {
            _aimDirection = aimDirection;
            _aimDirection.y = 0f;
            _movement = movement.normalized * Mathf.Clamp01(movement.magnitude);
            _movement.y = 0f;
            _fire = fire;
        }

        void Update()
        {
            Input.UpdateInput(this);

            if(_aimDirection.magnitude > 0.2f)
            {
                var targetRotation = Quaternion.LookRotation(_aimDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * Time.deltaTime
                );
            }

            if(_movement.magnitude > 0.2f)
            {
                SetAnimation("walk");
                ObjectLocator.Particles.WalkSmoke(transform.position, transform.forward, _rigidbody.velocity);
                if(_isHumanPlayer)
                {
                    FootStepsAudio.UnPause();
                }
            }
            else
            {
                SetAnimation("idle");
                
                if(_isHumanPlayer)
                {
                    FootStepsAudio.Pause();
                }
            }

            if(_fire)
            {
                TryFire();
                _fire = false;
            }
        }

        public void OnHitByCoin(Coin coin)
        {
            var damage = 2;
            var player = coin.FiredBy;
            
            while(damage > 0 && Budget > 0)
            {
                Budget--;
                damage--;
                var spawnedCoin = ObjectLocator.Coins.Pop();
                
                var target = transform.position;
                if(player != null)
                {
                    target = player.transform.position;
                }

                spawnedCoin.Loss(transform.position, target);
            }
            
            if(HitClip != null)
            {
                Audio.PlayOneShot(HitClip);
            }

            if(Budget <= 0)
            {
                Die();
            }
        }
        
        internal void PickUpCoin(Coin coin)
        {
            if(Dead) return;
            
            ObjectLocator.Particles.PickupCoin(coin.transform.position);
            if(PickupCoinClip != null)
            {
                Audio.PlayOneShot(PickupCoinClip);
            }
            Budget++;
        }

        private bool _alreadyDead;
        public void Die(bool skipEffects = false)
        {
            if(_alreadyDead) return;

            _alreadyDead = true;
            if(!skipEffects)
            {
                ObjectLocator.GameManager.OnPlayerDied(this);
                ObjectLocator.CameraShake.Shake();
                ObjectLocator.Particles.Explosion(transform.position, Vector3.up);
            }
            // TODO: GEtComponent GC
            ObjectLocator.DestructablePiggy.Execute(transform, GetComponentInChildren<MeshRenderer>().sharedMaterial);
            gameObject.SetActive(false);
        }

        private void TryFire()
        {
            if(Budget <= 0)
            {
                return;
            }

            if(ShootClip != null)
            {
                Audio.PlayOneShot(ShootClip);
            }

            var coin = ObjectLocator.Coins.Pop();
            coin.Fire(this, Muzzle);
            ObjectLocator.Particles.Explosion(Muzzle.position, Muzzle.forward);
            SetAnimation("shoot");
            Budget--;
            
            if(Budget <= 0)
            {
                Die();
            }
        }

        void FixedUpdate()
        {
            // Extra gravity
            _rigidbody.AddForce(Vector3.down * 10f, ForceMode.Force);

            _movement.y = 0f;
            _rigidbody.AddForce(_movement * Speed);

            var flatVelocity = _rigidbody.velocity;
            flatVelocity.y = 0f;

            _rigidbody.AddForce(-flatVelocity * MovementDamping);
            _rigidbody.angularVelocity = Vector3.zero;

        }
    }
}