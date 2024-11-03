using UnityEngine;

namespace LevelEditorPlugin.Runtime
{
    /// <summary>
    /// Manages the core movement mechanics of a character, including forward movement
    /// and jumping over obstacles. Designed to be extended or controlled by other scripts.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("The speed at which the character moves forward.")]
        [SerializeField] private float moveSpeed = 5f;

        [Tooltip("The distance the character should move forward when triggered.")]
        [SerializeField] private float movementDistance = 5f;

        [Tooltip("Additional height to clear obstacles during jumps.")]
        [SerializeField] private float bufferHeight = 0.5f;

        [Header("Obstacle Detection")]
        [Tooltip("Layer mask for detecting obstacles.")]
        [SerializeField] private LayerMask obstacleLayer;

        [Tooltip("Range of the raycast used to detect obstacles.")]
        [SerializeField] private float detectionRange = 1f;

        // Internal fields for movement and obstacle detection
        private Rigidbody2D rb;
        private bool isMoving = false;
        private bool isJumping = false;
        private float targetPositionX;

        private const float Gravity = 9.81f;

        /// <summary>
        /// Initializes the Rigidbody2D component.
        /// </summary>
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Handles forward movement and obstacle detection if character is moving.
        /// </summary>
        private void Update()
        {
            if (isMoving)
            {
                MoveForward();
                DetectObstacleAndJump();
            }
        }

        /// <summary>
        /// Starts the character's movement towards a target position.
        /// </summary>
        public void StartMoving()
        {
            targetPositionX = transform.position.x + movementDistance;
            isMoving = true;
        }

        /// <summary>
        /// Moves the character forward towards the target position.
        /// </summary>
        private void MoveForward()
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPositionX, transform.position.y), step);

            // Stop moving if the character reaches the target position
            if (Mathf.Abs(transform.position.x - targetPositionX) < 0.1f)
            {
                isMoving = false;
            }
        }

        /// <summary>
        /// Detects obstacles in front of the character and initiates a jump if needed.
        /// </summary>
        private void DetectObstacleAndJump()
        {
            if (isJumping) return;

            // Raycast to detect obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, obstacleLayer);
            Debug.DrawRay(transform.position, Vector2.right * detectionRange, Color.red);

            if (hit.collider != null)
            {
                float obstacleHeight = hit.collider.bounds.size.y;
                CalculateJumpForce(obstacleHeight + bufferHeight);
                Jump();
            }
        }

        /// <summary>
        /// Calculates the necessary jump force to clear an obstacle of a given height.
        /// </summary>
        /// <param name="jumpHeight">The height the character needs to jump.</param>
        private void CalculateJumpForce(float jumpHeight)
        {
            float jumpVelocity = Mathf.Sqrt(2 * Gravity * jumpHeight);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        /// <summary>
        /// Initiates the jump by setting the isJumping flag.
        /// </summary>
        private void Jump()
        {
            if (isJumping) return;
            isJumping = true;
        }

        /// <summary>
        /// Detects when the character lands on a surface and resets the jump state.
        /// </summary>
        /// <param name="collision">Collision information.</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Reset jumping state when landing on a horizontal surface
            if (collision.contacts[0].normal.y > 0.5f)
            {
                isJumping = false;
            }
        }
    }
}
