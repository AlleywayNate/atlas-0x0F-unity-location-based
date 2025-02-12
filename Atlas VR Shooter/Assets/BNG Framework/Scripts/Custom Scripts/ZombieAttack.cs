using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRange = 2f;          // How close the zombie needs to be to attack
    public float attackDamage = 10f;        // Damage dealt per attack
    public float attackCooldown = 1.5f;     // Time between attacks (cooldown)

    private float lastAttackTime = 0f;      // Tracks the last attack time
    private Animator animator;              // Reference to the Animator
    public Transform player;               // Reference to the player’s transform
    private PlayerHealth playerHealth;      // Reference to the player's health script

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is within attack range and cooldown has passed
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Trigger attack animation
        animator.SetTrigger("Attack");

        // Apply damage to the player
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // Reset the cooldown timer
        lastAttackTime = Time.time;
    }
}
