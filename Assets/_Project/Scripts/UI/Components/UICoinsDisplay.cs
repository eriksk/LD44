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
        private float _alpha;
        private Color _white, _red;

        void Start()
        {
            _alpha = CoinsText.color.a;
            _white = Color.white;
            _white.a = _alpha;
            _red = Color.red;
            _red.a = _alpha;

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
                CoinsText.color = _white;
                CoinsText.text = "0";
                return;
            }

            if(_previousBudget == Player.Budget) return;
            _previousBudget = Player.Budget;

            CoinsText.text = Player.Budget.ToString();
            if(Player.Budget < 7)
            {
                CoinsText.color = _red;
            }
            else
            {
                CoinsText.color = _white;
            }
        }

    }
}