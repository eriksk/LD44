
using System;
using System.Collections;
using LD44.Game.Players;
using LD44.UI.Components;
using UnityEngine;

namespace LD44.Game
{
    public class GameManager : MonoBehaviour
    {
        public GameState State;
        public UIGameOver UIGameOver;
        public AudioSource Audio;
        public AudioClip PlayerDestroyedClip;
        
        public GameObject PiggyPrefab;
        public PlayerInput HumanInput, CpuInput;
        public Material PlayerMaterial;
        public Material[] CpuMaterials;
        public float ElapsedTime;

        public Player Player { get; set; }

        void Start()
        {
            ObjectLocator.Clear();

            State = GameState.WaitingToStart;
            StartCoroutine(BeginCountDown());
        }

        private IEnumerator BeginCountDown()
        {
            SpawnPlayer();

            yield return new WaitForSeconds(1f);

            State = GameState.Playing;
            _timeUntilNextEnemySpawn = 3f;
        }

        private void SpawnPlayer()
        {
            var playerGameObject = Instantiate(PiggyPrefab);
            playerGameObject.transform.position = Vector3.zero;
            playerGameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            
            playerGameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial = PlayerMaterial;

            var player = playerGameObject.GetComponent<Player>();
            player.Input = HumanInput;
            player.Budget = 10;

            Player = player;
        }
        
        private void SpawnEnemy()
        {
            StartCoroutine(SpawnEnemyRoutine());
        }

        private IEnumerator SpawnEnemyRoutine()
        {
            var position = FindOptimalEnemyStartPosition();

            ObjectLocator.Particles.SpawnPlayer(position);

            var playerGameObject = Instantiate(PiggyPrefab);
            playerGameObject.transform.position = position;
            playerGameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            playerGameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial = CpuMaterials[UnityEngine.Random.Range(0, CpuMaterials.Length)];
            
            var player = playerGameObject.GetComponent<Player>();
            player.Input = CpuInput;
            player.Budget = 4;

            var dropDuration = 0.5f;
            var dropCurrent = 0f;

            while(dropCurrent <= dropDuration)
            {
                dropCurrent += Time.deltaTime;
                yield return null;

                var progress = Mathf.Pow(Mathf.Clamp01(dropCurrent / dropDuration), 2f);
                position.y = Mathf.Lerp(20f, 0f, progress);
                playerGameObject.transform.position = position;
            }

            ObjectLocator.Particles.PickupCoin(player.transform.position);
            ObjectLocator.CameraShake.Shake();
        }

        private Vector3 FindOptimalEnemyStartPosition()
        {
            var playerPosition = Player?.transform.position ?? Vector3.zero;

            var playerAngle = Mathf.Atan2(playerPosition.z, playerPosition.x) * Mathf.Rad2Deg;

            var targetAngle = (playerAngle + 180f + UnityEngine.Random.Range(-45f, 45f)) * Mathf.Deg2Rad;
            var distanceFromCenter = UnityEngine.Random.Range(2f, 10f);

            var position = new Vector3(
                Mathf.Cos(targetAngle),
                0f,
                Mathf.Sin(targetAngle)
            ) * distanceFromCenter;

            return position;
        }

        private float _timeUntilNextEnemySpawn = 0f;
        private float _intensity = 0f;

        void Update()
        {
            if(State == GameState.Playing)
            {
                ElapsedTime += Time.deltaTime;
                _intensity = Mathf.Lerp(0f, 1f, ElapsedTime / 60f); // Full intensity after a minute
                _timeUntilNextEnemySpawn -= Time.deltaTime;

                if(_timeUntilNextEnemySpawn <= 0f)
                {
                    SpawnEnemy();
                    _timeUntilNextEnemySpawn = 1f + UnityEngine.Random.Range(3f, 5f) * (1f - _intensity);
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
            UIGameOver.Show(ElapsedTime);
            yield return null;
        }

        public void OnPlayerDied(Player player)
        {
            if(PlayerDestroyedClip != null)
            {
                Audio.PlayOneShot(PlayerDestroyedClip);
            }
        }
    }

    public enum GameState
    {
        WaitingToStart,
        Playing,
        GameOver
    }
}