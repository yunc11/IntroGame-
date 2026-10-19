using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Leave empty - found automatically by the Player tag.")]
    [SerializeField] private Transform player;

    [Header("Cores")]
    [SerializeField] private int maxCores = 3;
    [SerializeField] private Material coreMaterial;  
    [SerializeField] private float orbitRadius = 1.2f;
    [SerializeField] private float orbitHeight = 1.2f;
    [SerializeField] private float orbitSpeed = 90f;  
    [SerializeField] private float orbSize = 0.35f;

    [Header("Detection")]
    [Tooltip("Extra enemy detection range per core. 0.2 = 1 core 1.2x, 2 cores 1.4x, 3 cores 1.6x")]
    [SerializeField] private float extraRangePerCore = 0.2f;

    public int Cores { get; private set; }
    public int Scrap { get; private set; }
    public int MaxCores => maxCores;
    public Transform Player => player;

    public float DetectionMultiplier => 1f + Cores * extraRangePerCore;

    public event Action OnInventoryChanged;

    private readonly List<Transform> orbs = new List<Transform>();
    private float angle;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else Debug.LogWarning("PlayerInventory: no object tagged 'Player' found.");
        }
    }

    public bool TryAddCore()
    {
        if (Cores >= maxCores) return false;   // hands full
        Cores++;
        SpawnOrb();
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void AddScrap(int amount)
    {
        Scrap += amount;
        OnInventoryChanged?.Invoke();
    }


    public int DepositCores()
        {
        int deposited = Cores;
        Cores = 0;

        foreach (Transform orb in orbs)
        {
            if (orb != null)
                Destroy(orb.gameObject);
        }

        orbs.Clear();

        OnInventoryChanged?.Invoke();

        return deposited;
        }

    private void SpawnOrb()
    {
        GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        orb.name = "CarriedCore";
        Destroy(orb.GetComponent<Collider>());  
        orb.transform.localScale = Vector3.one * orbSize;
        if (coreMaterial != null) orb.GetComponent<Renderer>().material = coreMaterial;
        orbs.Add(orb.transform);
    }

    private void Update()
    {
        if (player == null || orbs.Count == 0) return;

        angle += orbitSpeed * Time.deltaTime;
        float spacing = 360f / orbs.Count;

        for (int i = 0; i < orbs.Count; i++)
        {
            Vector3 offset = Quaternion.Euler(0f, angle + i * spacing, 0f) * Vector3.forward * orbitRadius;
            orbs[i].position = player.position + offset + Vector3.up * orbitHeight;
        }
    }
}
