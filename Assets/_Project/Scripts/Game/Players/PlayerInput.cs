
using UnityEngine;

namespace LD44.Game.Players
{
    public abstract class PlayerInput : ScriptableObject
    {
        public virtual void OnStart(Player player)
        {
        }
        public abstract void UpdateInput(Player player);
    }
}