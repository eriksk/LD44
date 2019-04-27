
using UnityEngine;

namespace LD44.Game.Players
{
    [CreateAssetMenu(menuName="Custom/Input/Human")]
    public class HumanPlayerInput : PlayerInput
    {
        public override void OnStart(Player player)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        public override void UpdateInput(Player player)
        {
            var tryingToFire = Input.GetButtonDown("Fire1");

            var inputDirection = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical")
            );

            var aimDirection = ObjectLocator.Crosshairs.GetDirection(player.transform.position);
            // TODO: use aim

            player.UpdateInput(inputDirection, aimDirection, tryingToFire);
        }
    }
}