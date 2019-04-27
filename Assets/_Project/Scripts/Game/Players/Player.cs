
using System;
using UnityEngine;

namespace LD44.Game.Players
{
    public class Player : MonoBehaviour
    {
        public float Speed = 1f;
        public float MovementDamping = 3f;
        public float RotationSpeed = 5f;

        public Animation Animator;
        public Transform Muzzle;
        public int Budget = 150;

        private string _currentAnimation;

        private Rigidbody _rigidbody;
        private Vector3 _movement;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetAnimation("idle", 0f);
        }

        private void SetAnimation(string name, float fadeLength = 0.1f)
        {
            if(_currentAnimation == name) return;
            _currentAnimation = name;

            Animator.CrossFade(_currentAnimation, fadeLength);
        }

        public void Move(Vector3 movement)
        {
            _movement = movement;
        }

        void Update()
        {
            var tryingToFire = Input.GetButtonDown("Fire1");

            var inputDirection = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical")
            );

            _movement = inputDirection.normalized * Mathf.Clamp01(inputDirection.magnitude);
            _movement.y = 0f;

            if(_movement.magnitude > 0.2f)
            {
                var direction = _movement.normalized;
                var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * Time.deltaTime
                );

                SetAnimation("walk");
                ObjectLocator.Particles.WalkSmoke(transform.position, transform.forward, _rigidbody.velocity);
            }
            else
            {
                SetAnimation("idle");
            }

            if(tryingToFire)
            {
                TryFire();
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
            SetAnimation("shoot");
            Budget--;
        }

        void FixedUpdate()
        {
            _rigidbody.AddForce(_movement * Speed);

            var flatVelocity = _rigidbody.velocity;
            flatVelocity.y = 0f;

            _rigidbody.AddForce(-flatVelocity * MovementDamping);
            _rigidbody.angularVelocity = Vector3.zero;

        }
    }
}