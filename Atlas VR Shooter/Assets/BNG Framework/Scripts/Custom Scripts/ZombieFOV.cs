using UnityEngine;
using UnityEngine.AI; // For NavMesh

public class ZombieFOV : MonoBehaviour
{
    public Transform player;               // Target to follow (the player)
    public float detectionRange = 15f;     // How far the zombie can detect the player
    public float stoppingDistance = 2f;    // Distance to stop before reaching the player
    public float fieldOfViewAngle = 110f;  // Field of view angle in degrees

    private NavMeshAgent agent;            // Handles pathfinding
    private Animator animator;             // Handles animations

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

        if (IsPlayerInFieldOfView() && distanceToPlayer <= detectionRange)
        {
            // Chase the player
            agent.SetDestination(player.position);

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
        agent.isStopped = distanceToPlayer <= stoppingDistance;
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true; // Player is visible
                }
            }
        }

        return false; // Player is not within FOV or obstructed
    }
}
