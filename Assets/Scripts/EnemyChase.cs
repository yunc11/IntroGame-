using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float detectionRange = 6f;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float nextAttackTime;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > stoppingDistance)
        {
            Vector3 targetPosition = new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
        else if (distanceToPlayer <= stoppingDistance)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (playerHealth == null || Time.time < nextAttackTime)
        {
            return;
        }

        playerHealth.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackCooldown;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
