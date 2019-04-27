
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD44.UI.Components
{
    public class UIGameOver : MonoBehaviour
    {
        public Text TextScore;
        public Text TextHighScore;
        public Text TipText;
        public Animation Animator;
        public AnimationClip Clip;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(float totalSecondsElapsed)
        {
            var highscore = PlayerPrefs.GetFloat("Highscore", totalSecondsElapsed);
            
            if(totalSecondsElapsed >= highscore)
            {
                PlayerPrefs.SetFloat("Highscore", totalSecondsElapsed);
                PlayerPrefs.Save();
            }

            gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Animator.Play(Clip.name, PlayMode.StopAll);
            TextScore.text = ((int)totalSecondsElapsed) + " seconds";
            TextHighScore.text = ((int)highscore) + " seconds";
            TipText.text = GetRandomTip();
        }

        private string GetRandomTip()
        {
            var tips = new[]
            {
                "Preserve your ammo, zero coins means game over!",
                "Try aiming better!",
                "Stay away from enemies to avoid being hit!",
                "Pick up coins fast, they disappear quickly!"
            };
            return "Tip: " + tips[UnityEngine.Random.Range(0, tips.Length)];
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}