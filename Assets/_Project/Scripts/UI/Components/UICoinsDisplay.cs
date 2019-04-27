using LD44.Game;
using LD44.Game.Players;
using UnityEngine;
using UnityEngine.UI;

namespace LD44.UI
{
    public class UICoinsDisplay : MonoBehaviour
    {
        public Text CoinsText;
        public Player Player;

        private int _previousBudget;

        void Start()
        {
            _previousBudget = -1;
        }

        public void Update()
        {
            if(ObjectLocator.GameManager == null)
            {
                return;
            }
            
            if(Player == null)
            {
                Player = ObjectLocator.GameManager.Player;
            }

            if(Player == null)
            {
                CoinsText.color = Color.white;
                CoinsText.text = "0";
                return;
            }

            if(_previousBudget == Player.Budget) return;
            _previousBudget = Player.Budget;

            CoinsText.text = Player.Budget.ToString();
            if(Player.Budget < 5)
            {
                CoinsText.color = Color.red;
            }
            else
            {
                CoinsText.color = Color.white;
            }
        }

    }
}