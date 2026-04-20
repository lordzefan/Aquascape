using UnityEngine;

public class Fish : Entity
{
    [Header("Fish Settings")]
    [Range(0f, 100f)]
    public float hunger = 100f;

    public float hungerDecreaseRate = 5f;
    public float detectionRadius = 5f;

    [Header("Fear Settings")]
    public float scareDuration = 2f;
    public float scareSpeedMultiplier = 2f;

    [Header("Eat Settings")]
    public float eatDistance = 0.25f;
    public float eatCooldown = 0.2f;

    [SerializeField] private GameObject bubble;

    private Food targetFood;

    private bool isScared = false;
    private float scareTimer = 0f;
    private float normalSpeed;
    private float eatTimer = 0f;

    protected override void Start()
    {
        base.Start();

        ApplyConfigSettings();

        normalSpeed = currentSpeed;
        hunger = 0f;
    }

    protected override void Update()
    {
        HandleTimers();
        HandleScareState();
        HandleHunger();

        if (isScared)
        {
            base.Update();
            return;
        }

        if (targetFood != null)
        {
            MoveToFood();
            CheckBoundary();
            CheckEatByDistance();
        }
        else
        {
            base.Update();
        }
    }

    /// <summary>
    /// Load fish settings from configuration data.
    /// </summary>
    private void ApplyConfigSettings()
    {
        if (ConfigManager.Data == null)
            return;

        minSpeed = ConfigManager.Data.fishMinSpeed;
        maxSpeed = ConfigManager.Data.fishMaxSpeed;
        detectionRadius = ConfigManager.Data.fishDetectionRadius;
        hungerDecreaseRate = ConfigManager.Data.fishHungerDecreaseRate;

        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    /// <summary>
    /// Update internal cooldown timers.
    /// </summary>
    private void HandleTimers()
    {
        if (eatTimer > 0f)
            eatTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Reduce hunger over time and search for food when starving.
    /// </summary>
    private void HandleHunger()
    {
        if (isScared)
            return;

        hunger -= hungerDecreaseRate * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 100f);

        if (hunger <= 0f && targetFood == null)
            SearchForFood();
    }

    /// <summary>
    /// Find the nearest food within detection range.
    /// </summary>
    private void SearchForFood()
    {
        Food[] foods = FindObjectsOfType<Food>();

        float closestDistance = detectionRadius;
        Food closestFood = null;

        foreach (Food food in foods)
        {
            if (food == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                food.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFood = food;
            }
        }

        targetFood = closestFood;
    }

    /// <summary>
    /// Move smoothly toward the targeted food.
    /// </summary>
    private void MoveToFood()
    {
        if (targetFood == null)
            return;

        Vector2 directionToFood =
            targetFood.transform.position - transform.position;

        if (directionToFood.magnitude <= 0.05f)
            return;

        Vector2 direction = directionToFood.normalized;

        moveDirection = Vector2.Lerp(
            moveDirection,
            direction,
            5f * Time.deltaTime
        ).normalized;

        Move();
    }

    /// <summary>
    /// Eat food when close enough.
    /// </summary>
    private void CheckEatByDistance()
    {
        if (targetFood == null || eatTimer > 0f)
            return;

        float distance = Vector2.Distance(
            transform.position,
            targetFood.transform.position
        );

        if (distance <= eatDistance)
            EatFood();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (eatTimer > 0f)
            return;

        Food food = other.GetComponent<Food>();

        if (food == null)
            return;

        targetFood = food;
        EatFood();
    }

    /// <summary>
    /// Consume targeted food and restore hunger.
    /// </summary>
    private void EatFood()
    {
        if (targetFood == null)
            return;

        Destroy(targetFood.gameObject);

        targetFood = null;
        hunger = 100f;
        eatTimer = eatCooldown;
    }

    /// <summary>
    /// Trigger fear behavior and increase movement speed temporarily.
    /// </summary>
    public void Scare()
    {
        Instantiate(bubble, transform);

        isScared = true;
        scareTimer = scareDuration;
        currentSpeed = normalSpeed * scareSpeedMultiplier;

        targetFood = null;

        SetRandomDirection();
    }

    /// <summary>
    /// Handle scare duration countdown.
    /// </summary>
    private void HandleScareState()
    {
        if (!isScared)
            return;

        scareTimer -= Time.deltaTime;

        if (scareTimer <= 0f)
        {
            isScared = false;
            currentSpeed = normalSpeed;
        }
    }
}