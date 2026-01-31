using UnityEngine;
using UnityEngine.Events;
using System;

namespace cowsins2D
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private Portal portalPrefab;
        [SerializeField] private Transform portalsParent;

        [SerializeField] private float creationCooldown = 1f;
        [SerializeField] private int maxPortals = 2;
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
            if (IsValidPosition(worldPos) == false)
            {
                return;
            }

            if (PortalManager.Instance.GetPortals().Count >= maxPortals)
            {
                PortalManager.Instance.ClearPortals();
            }
            
            PlacePortal(worldPos);
        }

        private bool IsValidPosition(Vector2 position)
        {
            return Physics2D.OverlapCircle(position, 0.2f, groundLayer) == false;
        }

        private void PlacePortal(Vector2 position)
        {
            Portal portal = Instantiate(portalPrefab, position, Quaternion.identity);
            portal.gameObject.SetActive(true);
            portal.transform.SetParent(portalsParent);
            portal.SetCreationTime(DateTime.Now);
            PortalManager.Instance.AddPortal(portal);
            lastCreation = portal.GetCreationTime();

            OnPortalPlaced?.Invoke();
        }

    }
}
