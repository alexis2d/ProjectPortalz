using UnityEngine;
using UnityEngine.Events;
using System;

namespace cowsins2D
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private Portal portalPrefab;
        [SerializeField] private float creationCooldown = 1f;
        [SerializeField] private LayerMask groundLayer;
        private UIController UIController;
        private PlayerDependencies playerDependencies;

        public UnityEvent OnPortalPlaced;

        private DateTime lastCreation;

        private void Start()
        {
            playerDependencies = FindObjectOfType<PlayerDependencies>();
            UIController = playerDependencies._UIController;
        }

        private void Update()
        {
            if (PlayerIsClicking() && UIController.highlightedInventorySlot == null && UIController.currentInventorySlot == null) HandlePortalPlacement();
        }

        private bool PlayerIsClicking()
        {
            return InputManager.PlayerInputs.Shoot;
        }

        private void HandlePortalPlacement()
        {
            TryPlacePortal();
        }

        public void TryPlacePortal()
        {
            if (DateTime.Now - lastCreation < TimeSpan.FromSeconds(creationCooldown))
            {
                return;
            }

            Vector2 screenPos = InputManager.PlayerInputs.MousePos;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Debug.Log(worldPos);

            if (IsValidPosition(worldPos) == false)
            {
                return;
            }
            
            PlacePortal(worldPos);
        }

        private bool IsValidPosition(Vector2 position)
        {
            return true;
        }

        private void PlacePortal(Vector2 position)
        {
            Instantiate(portalPrefab, position, Quaternion.identity);
            lastCreation = DateTime.Now;

            OnPortalPlaced?.Invoke();
        }
    }
}
