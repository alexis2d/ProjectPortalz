using UnityEngine;
using System.Collections;

namespace cowsins2D
{
    public class AuroraStats : MonoBehaviour
    {
        [ReadOnly, SerializeField] private float concentration;
        [SerializeField] private float maxConcentration;
        public float Concentration => concentration;
        public float MaxConcentration => maxConcentration;
        private AuroraDependencies auroraDependencies;
        private AuroraUIController auroraUIController;

        private void Start()
        {
            concentration = maxConcentration;
            auroraDependencies = GetComponent<AuroraDependencies>();
            auroraUIController = auroraDependencies._AuroraUIController;
            auroraUIController.onUpdateConcentration?.Invoke(concentration);
        }

    }
    
}