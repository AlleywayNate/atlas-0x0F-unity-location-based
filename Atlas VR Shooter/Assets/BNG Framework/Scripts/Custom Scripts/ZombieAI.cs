using UnityEngine;
using UnityEngine.AI; // For NavMesh

public class ZombieAI : MonoBehaviour
{
    public Transform player;           // Target to follow (the player)
    public float detectionRange = 15f; // How far the zombie can detect the player
    public float stoppingDistance = 2f; // Distance to stop before reaching the player

    private NavMeshAgent agent;        // Handles pathfinding
    private Animator animator;         // Handles animations

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing on " + gameObject.name);
            enabled = false; // Disable this script to prevent further errors
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Chase the player
            agent.SetDestination(player.position);

            // Trigger walking animation
            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop moving when player is out of range
            agent.ResetPath();

            if (animator != null)
                animator.SetBool("isWalking", false);
        }

        // Stop moving when close enough to attack
        if (distanceToPlayer <= stoppingDistance)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }
}
