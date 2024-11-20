using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Sprites for the different directions
    public Sprite upSprite1, upSprite2;
    public Sprite downSprite1, downSprite2;
    public Sprite leftSprite1, leftSprite2;
    public Sprite rightSprite1, rightSprite2;

    // Idle sprites (one for each direction)
    public Sprite upIdleSprite;
    public Sprite downIdleSprite;
    public Sprite leftIdleSprite;
    public Sprite rightIdleSprite;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 lastDirection;

    // Variables for sprite-switch interval
    public float switchInterval = 0.3f; // Time between sprite switches in seconds
    private float timeSinceLastSwitch = 0f;
    private bool useFirstSprite = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lastDirection = Vector2.down; // Default last direction is downward
    }

    void Update()
    {
        // Read input for horizontal and vertical movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement by prioritizing one direction
        if (moveX != 0) moveY = 0;

        // Set movement and normalize the vector
        movement = new Vector2(moveX, moveY).normalized;

        // If there is movement, update the last direction
        if (movement != Vector2.zero)
        {
            lastDirection = movement;

            // Timer for sprite switching
            timeSinceLastSwitch += Time.deltaTime;
            if (timeSinceLastSwitch >= switchInterval)
            {
                timeSinceLastSwitch = 0f; // Reset the timer
                useFirstSprite = !useFirstSprite; // Toggle between the first and second sprite
            }

            // Choose sprite based on movement direction and timing
            if (movement.x > 0)
            {
                spriteRenderer.sprite = useFirstSprite ? rightSprite1 : rightSprite2;
            }
            else if (movement.x < 0)
            {
                spriteRenderer.sprite = useFirstSprite ? leftSprite1 : leftSprite2;
            }
            else if (movement.y > 0)
            {
                spriteRenderer.sprite = useFirstSprite ? upSprite1 : upSprite2;
            }
            else if (movement.y < 0)
            {
                spriteRenderer.sprite = useFirstSprite ? downSprite1 : downSprite2;
            }
        }
        else
        {
            // Use idle sprite when the player stops
            if (lastDirection.x > 0)
            {
                spriteRenderer.sprite = rightIdleSprite;
            }
            else if (lastDirection.x < 0)
            {
                spriteRenderer.sprite = leftIdleSprite;
            }
            else if (lastDirection.y > 0)
            {
                spriteRenderer.sprite = upIdleSprite;
            }
            else if (lastDirection.y < 0)
            {
                spriteRenderer.sprite = downIdleSprite;
            }
        }
    }

    void FixedUpdate()
    {
        // Move the player by updating the Rigidbody2D's position
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
