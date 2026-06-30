using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace cowsins2D
{
    public class PortalController : MonoBehaviour
    {
        [SerializeField] private Portal portalPrefab;
        [SerializeField] private Transform portalsParent;
        [SerializeField] private int maxPortals = 2;
        [SerializeField] private float range = 20f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float slowMotionForce = 0.5f;
        [SerializeField] private float aimingCooldown = 2f;
        private float usedConcentration = 0;
        private UIController UIController;
        private PlayerDependencies playerDependencies;
        private AuroraDependencies auroraDependencies;
        private AuroraStats auroraStats;
        public UnityEvent OnPortalPlaced;
        private float aimingMaxDuration;
        private DateTime lastAimingStart;
        private DateTime lastAimingEnd;
        private bool aiming = false;
        public bool Aiming => aiming;

        private void Start()
        {
            playerDependencies = FindAnyObjectByType<PlayerDependencies>();
            auroraDependencies = FindAnyObjectByType<AuroraDependencies>();
            auroraStats = auroraDependencies.AuroraStats;
            UIController = playerDependencies._UIController;
            aimingMaxDuration = auroraStats.MaxConcentration;
            lastAimingStart = DateTime.Now - TimeSpan.FromSeconds(aimingMaxDuration);
            lastAimingEnd = DateTime.Now - TimeSpan.FromSeconds(aimingCooldown);
        }

        private void Update()
        {
            AutomaticRemoval();
            if (InputManager.PlayerInputs.Shoot && aiming && UIController.highlightedInventorySlot == null && UIController.currentInventorySlot == null) HandlePortalPlacement();
            if (InputManager.PlayerInputs.Reload && UIController.highlightedInventorySlot == null && UIController.currentInventorySlot == null) HandlePortalClearing();
            if (InputManager.PlayerInputs.Aiming && UIController.highlightedInventorySlot == null && UIController.currentInventorySlot == null)
            {
                HandlePortalAim(true);
            } else
            {
                HandlePortalAim(false);
            }
        }

        private void AutomaticRemoval()
        {
            List<Portal> portalsToRemove = new List<Portal>();
            foreach (var portal in PortalManager.Instance.GetPortals())
            {
                if (portal.RemovalWithTime() || portal.RemovalWithDistance(playerDependencies.gameObject.transform))
                {
                    portalsToRemove.Add(portal);
                }
            }
            int portalsToRemoveCount = portalsToRemove.Count;
            foreach (var portal in portalsToRemove)
            {
                PortalManager.Instance.RemovePortal(portal);
            }
            if (portalsToRemoveCount > 0)
            {
                PortalManager.Instance.ClearPortals();
            }
        }

        private void HandlePortalPlacement()
        {
            TryPlacePortal();
        }

        private void HandlePortalClearing()
        {
            PortalManager.Instance.ClearPortals();
        }

        private void HandlePortalAim(bool tryAiming)
        {
            bool canAim = auroraStats.Concentration > 0;
            
            if (tryAiming && canAim)
            {
                if (aiming == false)
                {
                    lastAimingStart = DateTime.Now;
                }
                usedConcentration = (float)(DateTime.Now - lastAimingStart).TotalSeconds;
                auroraStats.UpdateConcentration(usedConcentration);
                aiming = true;
                Cursor.lockState = CursorLockMode.None;
                Crosshair.Instance.Show();
                Time.timeScale = slowMotionForce;
            } else
            {
                if (aiming)
                {
                    lastAimingEnd = DateTime.Now;
                    auroraStats.UpdateLockedConcentration();
                }
                if (DateTime.Now - lastAimingEnd > TimeSpan.FromSeconds(aimingCooldown))
                {
                    auroraStats.ResetConcentration();
                }
                aiming = false;
                Cursor.lockState = CursorLockMode.Locked;
                Crosshair.Instance.Hide(false);
                Time.timeScale = 1f;
            }
        }

        public void TryPlacePortal()
        {
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
            if (Vector2.Distance(playerDependencies.gameObject.transform.position, position) > range)
            {
                return false;
            }
            return Physics2D.OverlapCircle(position, 0.2f, groundLayer) == false;
        }

        private void PlacePortal(Vector2 position)
        {
            Portal portal = Instantiate(portalPrefab, position, Quaternion.identity);
            portal.gameObject.SetActive(true);
            portal.transform.SetParent(portalsParent);
            portal.SetCreationTime(DateTime.Now);
            PortalManager.Instance.AddPortal(portal);

            OnPortalPlaced?.Invoke();
        }

    }
}
