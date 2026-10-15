using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private Renderer floor;

    [Header("Energy Cores")]
    [SerializeField] private GameObject corePrefab;
    [SerializeField] private int coreCount = 3;

    [Header("Scrap")]
    [SerializeField] private GameObject scrapPrefab;
    [SerializeField] private int scrapCount = 5;

    private void Start()
    {
        Spawn(corePrefab, coreCount);
        Spawn(scrapPrefab, scrapCount);
    }

    private void Spawn(GameObject prefab, int count)
    {
        if (prefab == null) return;

        Bounds area = floor.bounds;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(area.min.x, area.max.x),
                area.max.y + 0.6f,
                Random.Range(area.min.z, area.max.z));

            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
