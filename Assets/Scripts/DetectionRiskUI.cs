
using UnityEngine;
using TMPro;

public class DetectionRiskUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private TextMeshProUGUI riskText;

    private void Awake()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<PlayerInventory>();

        if (riskText == null)
            riskText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged += UpdateRisk;

        UpdateRisk();
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateRisk;
    }

    private void UpdateRisk()
    {
        if (inventory == null || riskText == null)
            return;

        int cores = inventory.Cores;

        if (cores == 0)
        {
            riskText.text = "DETECTION RISK: LOW";
            riskText.color = Color.green;
        }
        else if (cores == 1)
        {
            riskText.text = "DETECTION RISK: MEDIUM";
            riskText.color = Color.yellow;
        }
        else
        {
            riskText.text = "DETECTION RISK: HIGH";
            riskText.color = Color.red;
        }
    }
}
