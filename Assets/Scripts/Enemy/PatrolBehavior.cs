using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [SerializeField] float travelDistance = 0.5f;
    private Vector3 startPos;
    private bool moveRight = true;
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        startPos = transform.position;
        moveRight = true;
    }

    public void Patrol()
    {
        float speed = enemyMovement.GetComponent<EnemyMovement>().speed;
        float leftBound = startPos.x - travelDistance;
        float rightBound = startPos.x + travelDistance;

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