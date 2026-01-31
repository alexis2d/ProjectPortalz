using System;
using UnityEngine;

namespace cowsins2D
{
    public class Portal : Trigger
    {
        [SerializeField] private float usageCooldown = 2f;
        [SerializeField] private float lifetime = 20f;
        [SerializeField] private float distanceBeforeDestruction = 50f;
        private DateTime lastUsage;
        private DateTime creationTime;

        private void Update()
        {
            if (DateTime.Now - creationTime > TimeSpan.FromSeconds(lifetime))
            {
                PortalManager.Instance.RemovePortal(this);
                Destroy();
            }
        }

        public override void EnterTrigger(GameObject target)
        {
            Debug.Log("Entered Portal Trigger");
            if (canTeleport(target))
            {
                Teleport(target);
            }
            base.EnterTrigger(target);
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
            target.transform.position = exitPortal.transform.position;
            exitPortal.lastUsage = DateTime.Now;
            Debug.Log("Teleported to " + exitPortal.transform.position);
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
