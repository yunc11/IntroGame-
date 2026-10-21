using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Slider healthBar;

    private int currentHealth;
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        Debug.Log("Player health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        healthBar.value = currentHealth;

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log("Player has died!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GetComponent<PlayerMovement>().enabled = false;
    }
}