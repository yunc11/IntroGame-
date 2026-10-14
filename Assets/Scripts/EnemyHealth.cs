using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Enemy health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log("Enemy health: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }
}
