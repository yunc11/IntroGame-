using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionBoost : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [Tooltip("How often to look for newly spawned enemies (seconds).")]
    [SerializeField] private float rescanInterval = 1f;

    private readonly Dictionary<EnemyChase, float> baseRanges = new Dictionary<EnemyChase, float>();
    private readonly List<EnemyChase> deadEnemies = new List<EnemyChase>();
    private float nextScan;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<PlayerInventory>();
        if (inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        if (inventory != null) inventory.OnInventoryChanged += ApplyBoost;
    }

    private void OnDisable()
    {
        if (inventory != null) inventory.OnInventoryChanged -= ApplyBoost;
        RestoreOriginalRanges();
    }

    private void Update()
    {
        if (Time.time < nextScan) return;
        nextScan = Time.time + rescanInterval;
        ScanForEnemies();
    }

    private void ScanForEnemies()
    {
        foreach (EnemyChase enemy in FindObjectsByType<EnemyChase>(FindObjectsSortMode.None))
        {
            if (baseRanges.ContainsKey(enemy)) continue;
            baseRanges[enemy] = enemy.detectionRange;  
        }
        ApplyBoost();
    }

    private void ApplyBoost()
    {
        float multiplier = inventory != null ? inventory.DetectionMultiplier : 1f;
        deadEnemies.Clear();

        foreach (KeyValuePair<EnemyChase, float> entry in baseRanges)
        {
            if (entry.Key == null) { deadEnemies.Add(entry.Key); continue; }   // enemy was killed
            entry.Key.detectionRange = entry.Value * multiplier;
        }

        foreach (EnemyChase dead in deadEnemies) baseRanges.Remove(dead);
    }

    private void RestoreOriginalRanges()
    {
        foreach (KeyValuePair<EnemyChase, float> entry in baseRanges)
            if (entry.Key != null) entry.Key.detectionRange = entry.Value;
    }
}
