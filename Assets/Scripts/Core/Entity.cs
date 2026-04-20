using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Boundary")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Forward Avoidance")]
    public float frontCheckDistance = 0.45f;
    public float frontCheckRadius = 0.18f;
    public LayerMask obstacleLayer;

    [Header("Vertical Bias")]
    public float verticalAvoidLimit = 0.25f;

    protected float currentSpeed;
    protected Vector2 moveDirection;
    protected Rigidbody2D rb;

    private float directionTimer;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ConfigureRigidbody();
        InitializeMovement();
    }

    protected virtual void Update()
    {
        HandleDirectionChange();
        CheckBoundary();
    }

    protected virtual void FixedUpdate()
    {
        CheckForwardBlocked();
        Move();
    }

    /// <summary>
    /// Configure Rigidbody2D settings.
    /// </summary>
    private void ConfigureRigidbody()
    {
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    /// <summary>
    /// Initialize speed, direction, and turn timer.
    /// </summary>
    private void InitializeMovement()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        SetRandomDirection();

        directionTimer = Random.Range(2f, 4f);
    }

    /// <summary>
    /// Move entity smoothly in the current direction.
    /// </summary>
    protected virtual void Move()
    {
        Vector2 smoothDirection = Vector2.Lerp(
            moveDirection,
            moveDirection.normalized,
            7f * Time.fixedDeltaTime
        ).normalized;

        Vector2 nextPosition =
            rb.position +
            smoothDirection *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);

        moveDirection = smoothDirection;

        FlipSprite();
    }

    /// <summary>
    /// Flip sprite based on horizontal movement.
    /// </summary>
    private void FlipSprite()
    {
        if (Mathf.Abs(moveDirection.x) < 0.01f)
            return;

        Vector3 scale = transform.localScale;

        scale.x =
            Mathf.Abs(scale.x) *
            Mathf.Sign(moveDirection.x);

        transform.localScale = scale;
    }

    /// <summary>
    /// Set a random movement direction with limited vertical bias.
    /// </summary>
    protected void SetRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;

        moveDirection.y = Mathf.Clamp(
            moveDirection.y,
            -verticalAvoidLimit,
            verticalAvoidLimit
        );

        moveDirection.Normalize();

        if (moveDirection.magnitude < 0.1f)
            moveDirection = Vector2.right;
    }

    /// <summary>
    /// Change direction periodically.
    /// </summary>
    private void HandleDirectionChange()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer > 0f)
            return;

        TurnRandom();
        directionTimer = Random.Range(2f, 4f);
    }

    /// <summary>
    /// Apply a new random direction.
    /// </summary>
    private void TurnRandom()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;

        direction.y = Mathf.Clamp(
            direction.y,
            -verticalAvoidLimit,
            verticalAvoidLimit
        );

        moveDirection = direction.normalized;
    }

    /// <summary>
    /// Detect obstacles in front of the entity.
    /// </summary>
    private void CheckForwardBlocked()
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            rb.position,
            frontCheckRadius,
            moveDirection,
            frontCheckDistance,
            obstacleLayer
        );

        if (hit.collider == null)
            return;

        if (hit.collider.transform == transform)
            return;

        TurnSideways();
    }

    /// <summary>
    /// Turn sideways to avoid obstacles.
    /// </summary>
    private void TurnSideways()
    {
        float horizontalDirection =
            Random.value > 0.5f ? 1f : -1f;

        Vector2 direction = new Vector2(
            horizontalDirection,
            Random.Range(
                -verticalAvoidLimit,
                verticalAvoidLimit
            )
        );

        moveDirection = Vector2.Lerp(
            moveDirection,
            direction.normalized,
            0.95f
        ).normalized;

        directionTimer = Random.Range(1f, 2f);
    }

    /// <summary>
    /// Keep entity inside movement boundaries.
    /// </summary>
    protected void CheckBoundary()
    {
        Vector2 position = rb.position;
        bool hitBoundary = false;

        if (position.x < minX)
        {
            position.x = minX;
            hitBoundary = true;
        }
        else if (position.x > maxX)
        {
            position.x = maxX;
            hitBoundary = true;
        }

        if (position.y < minY)
        {
            position.y = minY;
            hitBoundary = true;
        }
        else if (position.y > maxY)
        {
            position.y = maxY;
            hitBoundary = true;
        }

        if (!hitBoundary)
            return;

        rb.position = position;
        TurnTowardCenter();
    }

    /// <summary>
    /// Redirect entity toward the center of the boundary area.
    /// </summary>
    private void TurnTowardCenter()
    {
        Vector2 center = new Vector2(
            (minX + maxX) * 0.5f,
            (minY + maxY) * 0.5f
        );

        Vector2 direction =
            (center - rb.position).normalized;

        direction.y = Mathf.Clamp(
            direction.y,
            -verticalAvoidLimit,
            verticalAvoidLimit
        );

        moveDirection = direction.normalized;
    }

    /// <summary>
    /// Draw forward obstacle detection gizmo.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position +
            (Vector3)(moveDirection * frontCheckDistance),
            frontCheckRadius
        );
    }
}