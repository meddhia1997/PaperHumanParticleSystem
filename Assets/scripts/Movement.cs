using UnityEngine;

public class PaperHumanMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;   // Speed of movement
    public float rotationSpeed = 200f; // Speed of rotation
    private Animator animator ;

    private Vector3 moveDirection; // Direction of movement
 private void Start()
 {
     animator = GetComponent<Animator>();

 }
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get input for movement (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal"); // Left-right movement
        float moveZ = Input.GetAxis("Vertical");   // Forward-backward movement

        // Calculate movement direction
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Move the paper human
        if (moveDirection.magnitude >= 0.1f)
        {
            // Move in the desired direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Smooth rotation towards movement direction
            //Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
