
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD44.UI.Sequences
{
    public class TitleScreenSequence : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}