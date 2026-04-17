using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    protected float currentSpeed;
    protected Vector2 moveDirection;

    private float directionTimer;

    [Header("Boundary")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Avoidance")]
    public float avoidanceRadius = 0.6f;
    public float avoidanceForce = 1f;

    protected virtual void Start()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        SetRandomDirection();

        directionTimer = Random.Range(1f, 3f);
    }

    protected virtual void Update()
    {
        AvoidOthers();
        Move();
        CheckBoundary();
        HandleDirectionChange();
    }

    // =========================
    // MOVEMENT
    // =========================
    protected virtual void Move()
    {
        moveDirection = Vector2.Lerp(
            moveDirection,
            moveDirection.normalized,
            2f * Time.deltaTime
        ).normalized;

        transform.Translate(
            moveDirection * currentSpeed * Time.deltaTime,
            Space.World
        );

        FlipSprite();
    }

    void FlipSprite()
    {
        if (moveDirection.x == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection.x);
        transform.localScale = scale;
    }

    protected void SetRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
    }

    // =========================
    // RANDOM DIRECTION CHANGE
    // =========================
    void HandleDirectionChange()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0f)
        {
            SetRandomDirection();
            directionTimer = Random.Range(1f, 3f);
        }
    }

    // =========================
    // BOUNDARY
    // =========================
    protected void CheckBoundary()
    {
        Vector3 pos = transform.position;
        bool hitWall = false;

        if (pos.x <= minX || pos.x >= maxX)
        {
            moveDirection.x = -moveDirection.x;
            moveDirection.y += Random.Range(-0.5f, 0.5f);

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            hitWall = true;
        }

        if (pos.y <= minY || pos.y >= maxY)
        {
            moveDirection.y = -moveDirection.y;
            moveDirection.x += Random.Range(-0.5f, 0.5f);

            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            hitWall = true;
        }

        if (hitWall)
        {
            moveDirection.Normalize();
        }

        transform.position = pos;
    }

    // =========================
    // ANTI OVERLAP
    // =========================
    void AvoidOthers()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            Entity other = hit.GetComponent<Entity>();

            if (other != null)
            {
                Vector2 away =
                    (transform.position - other.transform.position).normalized;

                moveDirection += away * avoidanceForce * Time.deltaTime;
            }
        }

        moveDirection.Normalize();
    }

    // =========================
    // DEBUG GIZMO
    // =========================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}