using UnityEngine;
using UnityEngine.AI; // For NavMesh

public class ZombieAI : MonoBehaviour
{
    public Transform player;           // Target to follow (the player)
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
        if (player == null) return; // Ensure the player reference is valid

        // Always follow the player
        agent.SetDestination(player.position);

        // Debugging: Draw a line from zombie to player every frame
        Debug.DrawLine(transform.position, player.position, Color.green);

        // Log distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // Trigger walking animation when moving
        if (animator != null)
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);

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

    // This will draw a small sphere at the player's position in the Scene view
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;  // Set color for the gizmo
            Gizmos.DrawSphere(player.position, 0.5f);  // Draw sphere at player's position
        }
    }
}
