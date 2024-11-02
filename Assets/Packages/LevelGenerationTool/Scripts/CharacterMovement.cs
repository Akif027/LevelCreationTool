using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float movementDistance = 5f; // How far the character should move after the event triggers
    [SerializeField] private float bufferHeight = 0.5f; // Extra height to ensure character clears the obstacle

    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float detectionRange = 1f; // Range of the raycast for obstacle detection

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isJumping = false;
    private float targetPositionX; // Position the character should reach after moving

    private const float Gravity = 9.81f; // Acceleration due to gravity

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveForward();
            DetectObstacleAndJump();
        }
    }

    public void StartMoving()
    {
        targetPositionX = transform.position.x + movementDistance;
        isMoving = true;
    }

    private void MoveForward()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPositionX, transform.position.y), step);

        if (Mathf.Abs(transform.position.x - targetPositionX) < 0.1f)
        {
            isMoving = false;
        }
    }

    private void DetectObstacleAndJump()
    {
        if (isJumping) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, obstacleLayer);
        Debug.DrawRay(transform.position, Vector2.right * detectionRange, Color.red);

        if (hit.collider != null)
        {
            float obstacleHeight = hit.collider.bounds.size.y;
            CalculateJumpForce(obstacleHeight + bufferHeight); // Add buffer to the obstacle height
            Jump();
        }
    }

    private void CalculateJumpForce(float jumpHeight)
    {
        // Use the physics formula: v = sqrt(2 * g * h)
        // where v is the jump velocity needed, g is gravity, and h is the height
        float jumpVelocity = Mathf.Sqrt(2 * Gravity * jumpHeight);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    private void Jump()
    {
        if (isJumping) return;

        isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the jumping state when character lands on the ground
        if (collision.contacts[0].normal.y > 0.5f) // Detects landing on a horizontal surface
        {
            isJumping = false;
        }
    }
}
