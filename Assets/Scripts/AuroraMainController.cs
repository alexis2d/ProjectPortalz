using UnityEngine;

namespace cowsins2D
{

    public class AuroraMainController : MonoBehaviour
    {

        private IPlayerMovement player;
        private PlayerDependencies playerDependencies;
        [SerializeField]
        private AuroraCameraController auroraCamera;
        private PlayerMultipliers multipliers;

        private void Start()
        {
            playerDependencies = GetComponent<PlayerDependencies>();
            player = playerDependencies.PlayerMovement;
            player.PlayerMovementEvents.onTurn.AddListener(ChangeOrientation);
            multipliers = GetComponent<PlayerMultipliers>();
            player.PlayerMovementEvents.onIdle.AddListener(ResetSpeedModifier);
            ChangeOrientation();
        }

        private void ChangeOrientation()
        {
            if (player.facingRight)
            {
                auroraCamera.ChangeOrientation(OrientationEnum.Right);
            }
            else
            {
                auroraCamera.ChangeOrientation(OrientationEnum.Left);
            }
        }

        private void ResetSpeedModifier()
        {
            multipliers.speedModifier = 1f;
        }
        
        private void OnDisable()
        {
            player.PlayerMovementEvents.onTurn.RemoveListener(ChangeOrientation);
            player.PlayerMovementEvents.onIdle.RemoveListener(ResetSpeedModifier);
        }

    }

}
