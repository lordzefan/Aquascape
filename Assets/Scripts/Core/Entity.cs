using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    protected float currentSpeed;
    protected Vector2 moveDirection;

    protected virtual void Start()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        SetRandomDirection();
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
    }

    protected void SetRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
    }
}
