using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        if (inventory != null) inventory.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (inventory != null) inventory.OnInventoryChanged -= Refresh;
    }

    private void Refresh()
    {
        if (inventory == null || text == null) return;
        text.text = $"Energy Cores: {inventory.Cores}/{inventory.MaxCores}\nScrap: {inventory.Scrap}";
    }
}
