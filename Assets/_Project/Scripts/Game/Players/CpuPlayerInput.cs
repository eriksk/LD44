
using UnityEngine;

namespace LD44.Game.Players
{
    [CreateAssetMenu(menuName="Custom/Input/CPU")]
    public class CpuPlayerInput : PlayerInput
    {
        public override void UpdateInput(Player player)
        {
            var target = ObjectLocator.GameManager.Player;

            if(target == null)
            {
                player.UpdateInput(Vector3.zero, Vector3.zero, false);
                return;
            }

            player.AIParameters.TimeUntilUpdateTarget -= Time.deltaTime;
            if(player.AIParameters.TimeUntilUpdateTarget <= 0f)
            {
                player.AIParameters.TimeUntilUpdateTarget = UnityEngine.Random.Range(0.2f, 2f);
                player.AIParameters.TargetPosition = target.transform.position;
            }

            var fire = false;
            var movement = Vector3.zero;
            var aim = player.transform.forward;

            var distance = Vector3.Distance(player.AIParameters.TargetPosition, player.transform.position);
            var direction = (player.AIParameters.TargetPosition - player.transform.position).normalized;

            if(distance < 5f && player.AIParameters.TimeSinceFired > 2f)
            {
                fire = true;
                player.AIParameters.TimeSinceFired = 0f;
            }
            if(distance < 15f && player.AIParameters.TimeSinceFired > 5f)
            {
                fire = true;
                player.AIParameters.TimeSinceFired = 0f;
            }
            else
            {
                player.AIParameters.TimeSinceFired += Time.deltaTime;
                movement = direction * 0.3f;
            }

            aim = direction;

            player.UpdateInput(movement, aim, fire);
        }
    }

    public class AIParameters
    {
        public float TimeSinceFired;
        public Vector3 TargetPosition;
        public float TimeUntilUpdateTarget;
    }
}