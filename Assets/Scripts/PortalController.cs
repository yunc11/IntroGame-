
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class PortalController : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private int coresRequired = 3;
    [SerializeField] private float interactionRadius = 3f;

    [Header("Portal Appearance")]
    [SerializeField] private Renderer portalSurface;
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;

    [Header("Portal UI")]
    [SerializeField] private TextMeshProUGUI portalPromptText;

    private PlayerInventory inventory;
    private Transform player;

    private int depositedCores = 0;
    private bool portalRepaired = false;
    private bool gameWon = false;
    [Header("Victory UI")]
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        inventory = FindFirstObjectByType<PlayerInventory>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        if (inventory == null)
            Debug.LogWarning("Portal: PlayerInventory not found.");

        if (player == null)
            Debug.LogWarning("Portal: Player not found.");

        UpdatePortalAppearance();
        UpdatePortalPrompt(false);
    }

    
    private void Update()
    {
        if (player == null)
            return;

        Vector3 playerPosition = player.position;
        Vector3 portalPosition = transform.position;

        playerPosition.y = 0f;
        portalPosition.y = 0f;

        float distance = Vector3.Distance(
            playerPosition,
            portalPosition
        );

        bool isNearPortal = distance <= interactionRadius;

        UpdatePortalPrompt(isNearPortal);

        if (!isNearPortal || gameWon)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!portalRepaired)
                DepositCores();
            else
                WinGame();
        }
    }


    private void DepositCores()
    {
        if (inventory == null)
            return;

        int deposited = inventory.DepositCores();

        if (deposited == 0)
        {
            Debug.Log("You have no Energy Cores to deposit!");
            return;
        }

        depositedCores += deposited;

        Debug.Log(
            "Energy Cores deposited: " +
            depositedCores + "/" + coresRequired
        );

        if (depositedCores >= coresRequired)
        {
            portalRepaired = true;
            UpdatePortalAppearance();

            Debug.Log(
                "Portal repaired! Press E to escape!"
            );
        }
    }

    private void UpdatePortalAppearance()
    {
        if (portalSurface == null)
            return;

        Material selectedMaterial =
            portalRepaired ? activeMaterial : inactiveMaterial;

        if (selectedMaterial != null)
            portalSurface.sharedMaterial = selectedMaterial;
    }

    
    private void WinGame()
    {
        gameWon = true;

        Debug.Log("YOU WIN! You escaped the dimension!");

        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        // Display the victory screen
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            interactionRadius
        );
    }


    private void UpdatePortalPrompt(bool isNearPortal)
    {
        if (portalPromptText == null)
            return;

        if (!isNearPortal || gameWon)
        {
            portalPromptText.gameObject.SetActive(false);
            return;
        }

        portalPromptText.gameObject.SetActive(true);

        if (portalRepaired)
        {
            portalPromptText.text = "Press E to escape!";
            portalPromptText.color = Color.cyan;
        }
        else if (inventory != null && inventory.Cores > 0)
        {
            portalPromptText.text = "Press E to deposit Energy Cores";
            portalPromptText.color = Color.white;
        }
        else
        {
            portalPromptText.text = "Collect Energy Cores to repair the portal";
            portalPromptText.color = Color.yellow;
        }
    }

}
