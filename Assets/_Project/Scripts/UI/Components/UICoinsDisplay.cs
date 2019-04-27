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
            if(Player == null) return;
            if(_previousBudget == Player.Budget) return;
            _previousBudget = Player.Budget;

            CoinsText.text = Player.Budget.ToString();
        }

    }
}