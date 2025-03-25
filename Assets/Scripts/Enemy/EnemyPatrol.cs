using UnityEngine;

public class EnemyPatrol : EnemyScript
{
    [SerializeField] public float travelDistance = 0.5f;

    protected override void Start()
    {
        base.Start();
        moveRight = true;
    }

    protected override void Patrol()
    {
        float leftBound = startPot.x - travelDistance;
        float rightBound = startPot.x + travelDistance;

        if (moveRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                moveRight = false;               
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                moveRight = true;               
            }
        }
    }
}
