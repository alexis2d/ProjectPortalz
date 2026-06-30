using UnityEngine;

namespace cowsins2D
{
    [DisallowMultipleComponent]
    public class AuroraDependencies : MonoBehaviour
    {
        [SerializeField] private AuroraUIController AuroraUIController;
        [SerializeField] private PortalController PortalController;
        public AuroraUIController _AuroraUIController => AuroraUIController;
        public PortalController _PortalController => PortalController;
        public AuroraStats AuroraStats { get; private set; }

        private void Awake()
        {
            AuroraStats = GetDependency<AuroraStats>();
        }

        public T GetDependency<T>() where T : class
        {
            var comp = GetComponent(typeof(T));
            if (comp == null)
            {
                Debug.LogWarning($"Dependency of type {typeof(T).Name} not found on {gameObject.name}.", this);
                return null;
            }

            var asT = comp as T;
            if (asT == null)
            {
                Debug.LogWarning($"Found component for {typeof(T).Name} but could not cast on {gameObject.name}.", this);
            }

            return asT;
        }
    }
}
