using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace cowsins2D
{
    public class AuroraUIController : MonoBehaviour
    {
        [SerializeField] private Image concentrationUI;
        [SerializeField] private float lerpBarValueSpeed;
        [SerializeField] private AuroraDependencies auroraDependencies;
        [SerializeField] private TextMeshProUGUI concentrationText;
        private float targetConcentrationValue;
        private AuroraStats auroraStats;
        public delegate void OnUpdateConcentration(float concentration);
        public OnUpdateConcentration onUpdateConcentration;

        private void Start()
        {
            auroraStats = auroraDependencies.AuroraStats;
            if (concentrationText != null) Destroy(concentrationText.gameObject);
            onUpdateConcentration = BarConcentration;
        }

        private void Update()
        {
            if (concentrationUI != null)
            {
                // Lerp the shield UI fill amount towards the target shield value
                concentrationUI.fillAmount = Mathf.Lerp(concentrationUI.fillAmount, targetConcentrationValue / auroraStats.MaxConcentration, lerpBarValueSpeed * Time.deltaTime);
            }
        }

        private void BarConcentration(float concentration)
        {
            targetConcentrationValue = concentration;
        }

        private void OnDisable()
        {
            onUpdateConcentration = null;
        }


    }
}