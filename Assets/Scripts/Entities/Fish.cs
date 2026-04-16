using UnityEngine;

public class Fish : Entity
{
    [Header("Fish Settings")]
    public float hunger = 100f;
    public float hungerDecreaseRate = 5f;
    public float detectionRadius = 2f;

    private Food targetFood;
    private bool isScared = false;


    protected override void Start()
    {
        base.Start();
        hunger = 0; // langsung lapar saat spawn
    }
    protected override void Update()
    {
        HandleHunger();

        if (targetFood != null)
        {
            MoveToFood();
        }
        else
        {
            base.Update(); // random movement
        }
    }

    void MoveToFood()
    {
        if (targetFood == null) return;
        Debug.Log("Moving to food"); // DEBUG

        Vector2 direction = (targetFood.transform.position - transform.position).normalized;

        transform.Translate(direction * currentSpeed * Time.deltaTime);

        float dist = Vector2.Distance(transform.position, targetFood.transform.position);

        if (dist < 0.2f)
        {
            EatFood();
        }
    }

    void EatFood()
    {
        Destroy(targetFood.gameObject);
        targetFood = null;

        hunger = 100f;
    }

    void HandleHunger()
    {
        hunger -= hungerDecreaseRate * Time.deltaTime;
        hunger = Mathf.Clamp(hunger, 0, 100);

        if (hunger <= 0 && targetFood == null)
        {
            Debug.Log("Fish is searching for food");
            SearchForFood();
        }
    }
    void SearchForFood()
    {
        Food[] foods = FindObjectsOfType<Food>();
        Debug.Log("Food found: " + foods.Length); // DEBUG
        float closestDistance = detectionRadius;
        Food closest = null;

        foreach (var food in foods)
        {
            float dist = Vector2.Distance(transform.position, food.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = food;
            }
        }

        targetFood = closest;
    }

    public void Scare()
    {
        isScared = true;
        SetRandomDirection();
        Invoke(nameof(StopScare), 2f);
    }

    void StopScare()
    {
        isScared = false;
    }
}