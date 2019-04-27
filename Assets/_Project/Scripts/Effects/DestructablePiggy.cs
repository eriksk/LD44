using System;
using System.Collections;
using System.Collections.Generic;
using LD44.Collections.Pool;
using LD44.Data;
using UnityEngine;

namespace LD44.Effects
{
    public class DestructablePiggy : MonoBehaviour, IPoolable
    {
        public List<Rigidbody> Nodes;

        private Dictionary<int, TransformState> _states;
        private Coroutine _destroyCoroutine;

        [HideInInspector]
        public bool Dead;

        void Start()
        {
        }

        public void Initiate()
        {
            _states = new Dictionary<int, TransformState>();
            foreach(var node in Nodes)
            {
                _states.Add(node.GetInstanceID(), TransformState.Create(node.transform));
            }
        }

        public void Reset()
        {
            Dead = false;

            foreach(var node in Nodes)
            {
                _states[node.GetInstanceID()].Restore(node.transform);
                node.velocity = Vector3.zero;
                node.angularVelocity = Vector3.zero;
                node.transform.localScale = Vector3.one;
            }

            if(_destroyCoroutine != null)
            {
                StopCoroutine(_destroyCoroutine);
            }
            _destroyCoroutine = StartCoroutine(WaitAndDestroy());
        }

        public void Explode()
        {
            foreach(var node in Nodes)
            {
                var direction = new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(0f, 1f),
                    UnityEngine.Random.Range(-1f, 1f)
                );
                node.AddForce(direction * UnityEngine.Random.Range(1f, 3f), ForceMode.Impulse);
            }
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(3f);
            
            var current = 0f;
            var fadeOutTime = 1f;

            while(current <= fadeOutTime)
            {
                current += Time.deltaTime;

                foreach(var node in Nodes)
                {
                    node.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Clamp01(current / fadeOutTime));
                }

                yield return null;
            }

            _destroyCoroutine = null;
            Dead = true;
        }
    }
}