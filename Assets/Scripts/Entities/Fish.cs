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

    private Food targetFood;

    private bool isScared = false;
    private float scareTimer = 0f;

    private float normalSpeed;
    private float eatTimer = 0f;

    [SerializeField]
    private GameObject bubble;

    // =====================================
    // START
    // =====================================
    protected override void Start()
    {
        base.Start();
        if (ConfigManager.Data != null)
        {
            minSpeed = ConfigManager.Data.fishMinSpeed;
            maxSpeed = ConfigManager.Data.fishMaxSpeed;

            detectionRadius =
                ConfigManager.Data.fishDetectionRadius;

            hungerDecreaseRate =
                ConfigManager.Data.fishHungerDecreaseRate;

            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }

        normalSpeed = currentSpeed;

        // Untuk test cepat. Kalau mau normal, ubah ke 100.
        hunger = 0f;
    }

    // =====================================
    // UPDATE
    // =====================================
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
            if (targetFood == null)
            {
                targetFood = null;
                base.Update();
                return;
            }

            MoveToFood();
            CheckBoundary();
            CheckEatByDistance();
        }
        else
        {
            base.Update();
        }
    }

    // =====================================
    // TIMER
    // =====================================
    void HandleTimers()
    {
        if (eatTimer > 0f)
            eatTimer -= Time.deltaTime;
    }

    // =====================================
    // HUNGER
    // =====================================
    void HandleHunger()
    {
        if (isScared) return;

        hunger -= hungerDecreaseRate * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0f, 100f);

        if (hunger <= 0f && targetFood == null)
        {
            SearchForFood();
        }
    }

    // =====================================
    // SEARCH FOOD
    // =====================================
    void SearchForFood()
    {
        Food[] foods = FindObjectsOfType<Food>();

        float closestDistance = detectionRadius;
        Food closest = null;

        foreach (Food food in foods)
        {
            if (food == null) continue;

            float dist = Vector2.Distance(
                transform.position,
                food.transform.position
            );

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = food;
            }
        }

        targetFood = closest;
    }

    // =====================================
    // MOVE TO FOOD
    // =====================================
    void MoveToFood()
    {
        if (targetFood == null) return;

        Vector2 toFood =
            targetFood.transform.position - transform.position;

        if (toFood.magnitude <= 0.05f)
            return;

        Vector2 dir = toFood.normalized;

        moveDirection = Vector2.Lerp(
            moveDirection,
            dir,
            5f * Time.deltaTime
        ).normalized;

        Move();
    }

    // =====================================
    // EAT BY DISTANCE
    // =====================================
    void CheckEatByDistance()
    {
        if (targetFood == null) return;
        if (eatTimer > 0f) return;

        float dist = Vector2.Distance(
            transform.position,
            targetFood.transform.position
        );

        if (dist <= eatDistance)
        {
            EatFood();
        }
    }

    // =====================================
    // TRIGGER EAT
    // =====================================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (eatTimer > 0f) return;

        Food food = other.GetComponent<Food>();

        if (food != null)
        {
            targetFood = food;
            EatFood();
        }
    }

    // =====================================
    // EAT
    // =====================================
    void EatFood()
    {
        if (targetFood == null) return;

        Destroy(targetFood.gameObject);

        targetFood = null;

        hunger = 100f;
        eatTimer = eatCooldown;
    }

    // =====================================
    // FEAR
    // =====================================
    public void Scare()
    {
        Instantiate(bubble,gameObject.transform);
        isScared = true;
        scareTimer = scareDuration;

        currentSpeed = normalSpeed * scareSpeedMultiplier;

        targetFood = null;

        SetRandomDirection();
    }

    void HandleScareState()
    {
        if (!isScared) return;

        scareTimer -= Time.deltaTime;

        if (scareTimer <= 0f)
        {
            isScared = false;
            currentSpeed = normalSpeed;
        }
    }
}