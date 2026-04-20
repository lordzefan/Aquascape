using UnityEngine;

public class Trash : Entity
{
    protected override void Start()
    {
        base.Start();

        ApplyConfigSettings();
    }

    /// <summary>
    /// Load trash movement settings from configuration data.
    /// </summary>
    private void ApplyConfigSettings()
    {
        if (ConfigManager.Data == null)
            return;

        minSpeed = ConfigManager.Data.trashMinSpeed;
        maxSpeed = ConfigManager.Data.trashMaxSpeed;

        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    /// <summary>
    /// Move trash with smoother drifting motion and floating effect.
    /// </summary>
    protected override void Move()
    {
        Vector2 currentDirection =
            rb.linearVelocity.magnitude > 0.05f
                ? rb.linearVelocity.normalized
                : moveDirection;

        Vector2 smoothDirection = Vector2.Lerp(
            currentDirection,
            moveDirection.normalized,
            2f * Time.fixedDeltaTime
        ).normalized;

        if (smoothDirection.magnitude < 0.1f)
            smoothDirection = moveDirection.normalized;

        Vector2 nextPosition =
            rb.position +
            smoothDirection *
            currentSpeed *
            Time.fixedDeltaTime;

        ApplyFloatingEffect(ref nextPosition);
        ClampToBoundary(ref nextPosition);

        rb.MovePosition(nextPosition);

        moveDirection = smoothDirection;
    }

    /// <summary>
    /// Add subtle vertical floating motion.
    /// </summary>
    private void ApplyFloatingEffect(ref Vector2 position)
    {
        float floatOffset =
            Mathf.Sin(Time.time * 1.5f) * 0.01f;

        position.y += floatOffset;
    }

    /// <summary>
    /// Keep trash inside allowed boundaries.
    /// </summary>
    private void ClampToBoundary(ref Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
    }
}