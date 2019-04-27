
using UnityEngine;

namespace LD44.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool InMask(this GameObject gameObject, LayerMask mask)
        {
            return mask == (mask | (1 << gameObject.layer));
        }
    }
}