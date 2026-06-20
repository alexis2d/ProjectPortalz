using System;
using System.Collections;
using UnityEngine;

namespace cowsins2D
{
    public class Portal : Trigger
    {
        [SerializeField] private float usageCooldown = 2f;
        [SerializeField] private float lifetime = 20f;
        [SerializeField] private float distanceBeforeDestruction = 50f;
        [SerializeField] private float teleportSpeedBoost = 2f;
        [SerializeField] private float maxSpeedModifier = 10f;
        private DateTime lastUsage;
        private DateTime creationTime;

        public override void EnterTrigger(GameObject target)
        {
            Debug.Log("Entered Portal Trigger");
            if (canTeleport(target))
            {
                Teleport(target);
            }
            base.EnterTrigger(target);
        }

        public bool RemovalWithTime()
        {
            if (DateTime.Now - creationTime > TimeSpan.FromSeconds(lifetime))
            {
                Destroy();
                return true;
            }
            return false;
        }

        public bool RemovalWithDistance(Transform player)
        {
            if (Vector2.Distance(player.position, transform.position) > distanceBeforeDestruction)
            {
                Destroy();
                return true;
            }
            return false;
        }

        private bool canTeleport(GameObject target)
        {
            if (DateTime.Now - lastUsage < TimeSpan.FromSeconds(usageCooldown))
            {
                return false;
            }
            return true;
        }

        private void Teleport(GameObject target)
        {
            Portal exitPortal = PortalManager.Instance.GetExitPortal(this);
            if (exitPortal != null)
            {
                target.transform.position = exitPortal.transform.position;
                exitPortal.lastUsage = DateTime.Now;
                
                PlayerMultipliers multipliers = target.GetComponent<PlayerMultipliers>();
                if (multipliers != null && multipliers.speedModifier < maxSpeedModifier)
                {
                    multipliers.speedModifier += teleportSpeedBoost;
                }
            }
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }

        public void SetCreationTime(DateTime time)
        {
            creationTime = time;
        }

        public DateTime GetCreationTime()
        {
            return creationTime;
        }

    }
}
