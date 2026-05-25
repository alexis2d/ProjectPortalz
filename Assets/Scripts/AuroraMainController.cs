using UnityEngine;

namespace cowsins2D
{

    public class AuroraMainController : MonoBehaviour
    {

        private IPlayerMovement player;
        private PlayerDependencies playerDependencies;
        [SerializeField] private AuroraCameraController auroraCamera;

        private void Start()
        {
            playerDependencies = GetComponent<PlayerDependencies>();
            player = playerDependencies.PlayerMovement;
            player.PlayerMovementEvents.onTurn.AddListener(ChangeOrientation);
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
        
        private void OnDisable()
        {
            player.PlayerMovementEvents.onTurn.RemoveListener(ChangeOrientation);
        }

    }

}
