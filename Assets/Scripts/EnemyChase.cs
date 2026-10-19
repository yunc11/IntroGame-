
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float detectionRange = 6f;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Safe Zone")]
    [SerializeField] private float safeZoneBoundaryX = -10f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float nextAttackTime;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        // The green Safe Zone ends at X = -10.
        // Enemies must not enter or attack inside it.
        if (player.position.x < safeZoneBoundaryX)
            return;

        Vector3 targetPosition = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange &&
            distanceToPlayer > stoppingDistance)
        {
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

        // Prevent enemies crossing into the Safe Zone.
        Vector3 position = transform.position;
        position.x = Mathf.Max(
            position.x,
            safeZoneBoundaryX + 0.5f
        );
        transform.position = position;
    }

    private void AttackPlayer()
    {
        if (playerHealth == null ||
            Time.time < nextAttackTime)
            return;

        if (player.position.x < safeZoneBoundaryX)
            return;

        playerHealth.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );
    }
}
