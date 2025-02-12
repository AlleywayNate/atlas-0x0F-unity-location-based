using UnityEngine;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    private Vector3 lastPosition;  // Store the last position of the player
    public float movementThreshold = 0.1f;  // How much the player must move to be considered moving

    void Start()
    {
        lastPosition = transform.position;  // Initialize last position at the start
    }

    void Update()
    {
        // Check if the player is moving
        bool isMoving = Vector3.Distance(lastPosition, transform.position) > movementThreshold;

        // Log if the player is moving
        if (isMoving)
        {
//           Debug.Log("Player is moving!");
        }
        else
        {
//            Debug.Log("Player is standing still.");
        }

        // Draw a ray from the player's position to visualize it in the scene
        Debug.DrawRay(transform.position, Vector3.up * 0.5f, Color.green);

        // Optionally log the player's current position to check where they are in the world
//        Debug.Log("Player Position: " + transform.position);

        // Update last position for the next frame
        lastPosition = transform.position;
    }
}
