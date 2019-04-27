
using System;
using UnityEngine;

namespace LD44.Game.Players
{
    public class Player : MonoBehaviour
    {
        public float Speed = 1f;
        public float MovementDamping = 3f;
        public float RotationSpeed = 5f;

        public PlayerInput Input;

        public Animation Animator;
        public Transform Muzzle;
        public int Budget = 150;

        private string _currentAnimation;

        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Vector3 _aimDirection;
        private bool _fire;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetAnimation("idle", 0f);
            Input.OnStart(this);
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
            }
            else
            {
                SetAnimation("idle");
            }

            if(_fire)
            {
                TryFire();
                _fire = false;
            }
        }

        private void TryFire()
        {
            if(Budget <= 0)
            {
                return;
            }
            var coin = ObjectLocator.Coins.Pop();
            coin.Fire(this, Muzzle);
            ObjectLocator.Particles.Explosion(Muzzle.position, Muzzle.forward);
            SetAnimation("shoot");
            Budget--;
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