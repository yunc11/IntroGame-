using UnityEngine;

public enum PickupType { EnergyCore, Scrap }

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType type = PickupType.EnergyCore;
    [SerializeField] private int scrapAmount = 1;
    [SerializeField] private float pickupRadius = 1.2f;
    [SerializeField] private float spinSpeed = 90f;

    private PlayerInventory inventory;

    private void Start()
    {
        inventory = FindFirstObjectByType<PlayerInventory>();
        if (inventory == null) Debug.LogWarning("Pickup: no PlayerInventory in the scene.");

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        if (inventory == null || inventory.Player == null) return;

        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = inventory.Player.position; b.y = 0f;

        if (Vector3.Distance(a, b) <= pickupRadius) TryCollect();
    }

    private void TryCollect()
    {
        if (type == PickupType.EnergyCore)
        {
            if (inventory.TryAddCore()) Destroy(gameObject);   // core stays on the ground if already carrying 3
        }
        else
        {
            inventory.AddScrap(scrapAmount);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
