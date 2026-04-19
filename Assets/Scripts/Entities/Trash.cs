using UnityEngine;

public class Trash : Entity
{
    protected override void Start()
    {
        base.Start();

        if (ConfigManager.Data != null)
        {
            minSpeed = ConfigManager.Data.trashMinSpeed;
            maxSpeed = ConfigManager.Data.trashMaxSpeed;

            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }
    }
    protected override void Move()
    {
        // gerakan lebih lambat dan smooth
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);

        // optional: sedikit naik turun
        transform.position += new Vector3(0, Mathf.Sin(Time.time) * 0.001f, 0);
    }
}