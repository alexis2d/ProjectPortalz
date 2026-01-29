using UnityEngine;
using System.Collections.Generic;


namespace cowsins2D
{
    public class PortalManager : MonoBehaviour
    {
        private static PortalManager instance;
        public static PortalManager Instance { get { return instance; } }
        private List<Portal> portals = new List<Portal>();


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public List<Portal> GetPortals()
        {
            return portals;
        }

        public void AddPortal(Portal portal)
        {
            portals.Add(portal);
        }

        public Portal GetExitPortal(Portal entryPortal)
        {
            foreach (var portal in portals)
            {
                if (portal != entryPortal)
                {
                    return portal;
                }
            }
            return null;
        }

        public void ClearPortals()
        {
            foreach (var portal in portals)
            {
                portal.Destroy();
            }
            portals.Clear();
        }

    }
}