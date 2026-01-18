using UnityEngine;

namespace cowsins2D
{
    public class PortalManager : MonoBehaviour
    {
        private static PortalManager instance;
        public static PortalManager Instance { get { return instance; } }
        private Portal[] portals;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                SetPortals();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void SetPortals()
        {
            portals = FindObjectsOfType<Portal>();
            foreach (var portal in portals)
            {
                Debug.Log(portal.transform.position);
            }
        }

        public Portal GetExitPortal(Portal entryPortal)
        {
            foreach (var portal in portals)
            {
                if (portal != entryPortal)
                {
                    Debug.Log(portal);
                    return portal;
                }
            }
            return null;
        }

    }
}