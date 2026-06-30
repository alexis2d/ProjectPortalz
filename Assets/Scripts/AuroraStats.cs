using UnityEngine;
using System.Collections;

namespace cowsins2D
{
    public class AuroraStats : MonoBehaviour
    {
        [ReadOnly, SerializeField] private float concentration;
        [SerializeField] private float maxConcentration;
        private float lockedConcentration;
        public float Concentration => concentration;
        public float MaxConcentration => maxConcentration;
        private AuroraDependencies auroraDependencies;
        private AuroraUIController auroraUIController;
        private PortalController portalController;

        private void Start()
        {
            concentration = maxConcentration;
            lockedConcentration = maxConcentration;
            auroraDependencies = GetComponent<AuroraDependencies>();
            auroraUIController = auroraDependencies._AuroraUIController;
            auroraUIController.onUpdateConcentration?.Invoke(concentration);
        }

        public void UpdateConcentration(float usedConcentration)
        {
            concentration = lockedConcentration - usedConcentration;
            auroraUIController.onUpdateConcentration?.Invoke(concentration);
        }

        public void UpdateLockedConcentration()
        {
            lockedConcentration = concentration;
        }

        public void ResetConcentration()
        {
            concentration = maxConcentration;
            UpdateLockedConcentration();
            auroraUIController.onUpdateConcentration?.Invoke(concentration);
        }

    }
    
}