
using System;
using System.Collections;
using LD44.Game.Players;
using UnityEngine;

namespace LD44.Game
{
    public class GameManager : MonoBehaviour
    {
        public GameState State;
        public GameObject PiggyPrefab;
        public PlayerInput HumanInput, CpuInput;
        public float ElapsedTime;

        public Player Player { get; set; }

        void Start()
        {
            State = GameState.WaitingToStart;
            StartCoroutine(BeginCountDown());
        }

        private IEnumerator BeginCountDown()
        {
            yield return new WaitForSeconds(1f);

            SpawnPlayer();

            // TODO: Countdown
            // Debug.Log("3");
            // yield return new WaitForSeconds(1f);
            // Debug.Log("2");
            // yield return new WaitForSeconds(1f);
            // Debug.Log("1");
            // yield return new WaitForSeconds(1f);

            State = GameState.Playing;
        }

        private void SpawnPlayer()
        {
            var playerGameObject = Instantiate(PiggyPrefab);
            playerGameObject.transform.position = Vector3.zero;
            playerGameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
            var player = playerGameObject.GetComponent<Player>();
            player.Input = HumanInput;
            player.Budget = 10;

            Player = player;
        }
        
        private void SpawnEnemy()
        {
            var playerGameObject = Instantiate(PiggyPrefab);
            playerGameObject.transform.position = new Vector3(
                UnityEngine.Random.Range(-5f, 5f),
                0f,
                UnityEngine.Random.Range(-5f, 5f)
            );
            playerGameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
            var player = playerGameObject.GetComponent<Player>();
            player.Input = CpuInput;
            player.Budget = 4;

            ObjectLocator.Particles.Explosion(player.transform.position, Vector3.up);
        }

        private float _timeSinceSpawned = 0f;

        void Update()
        {
            if(State == GameState.Playing)
            {
                ElapsedTime += Time.deltaTime;
                _timeSinceSpawned += Time.deltaTime;

                if(_timeSinceSpawned > 5f)
                {
                    SpawnEnemy();
                    _timeSinceSpawned = 0f;
                }

                if(Player == null || Player.Dead)
                {
                    GameOver();
                    return;
                }
            }
        }

        private void GameOver()
        {
            State = GameState.GameOver;
            StartCoroutine(BeginGameOver());
        }

        private IEnumerator BeginGameOver()
        {
            Debug.Log("GAME OVER YALL, result: " + ElapsedTime);
            yield return new WaitForSeconds(3f);
        }
    }

    public enum GameState
    {
        WaitingToStart,
        Playing,
        GameOver
    }
}