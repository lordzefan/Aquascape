using UnityEngine;

public class Trash : Entity
{
    protected override void Move()
    {
        // gerakan lebih lambat dan smooth
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);

        // optional: sedikit naik turun
        transform.position += new Vector3(0, Mathf.Sin(Time.time) * 0.001f, 0);
    }
}