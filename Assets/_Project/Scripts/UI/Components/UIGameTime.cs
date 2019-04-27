using System;
using LD44.Game;
using UnityEngine;
using UnityEngine.UI;

namespace LD44.UI.Components
{
    public class UIGameTime : MonoBehaviour
    {
        public Text MinutesText;
        public Text SecondsText;

        private string[] _numbers;

        void Start()
        {
            _numbers = new string[100];
            for(var i = 0; i < _numbers.Length; i++)
            {
                _numbers[i] = i.ToString().PadLeft(2, '0');
            }
        }

        void Update()
        {
            var time = ObjectLocator.GameManager.ElapsedTime;
            var timeSpan = TimeSpan.FromSeconds(time);

            MinutesText.text = _numbers[timeSpan.Minutes];
            SecondsText.text = _numbers[timeSpan.Seconds];
        }
    }
}