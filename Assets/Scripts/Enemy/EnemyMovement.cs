using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float speed = 1f;
    [SerializeField] public float detectionRange = 5f;
    protected GameObject player;
    protected SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;
    private float minMoveDistance = 0.1f;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    public void UpdateMovement(System.Action movementAction)
    {
        if (player == null) return;

        movementAction?.Invoke();

        Vector2 movementDirection = (transform.position - lastPosition).normalized;
        if (movementDirection.magnitude > minMoveDistance)
        {
            FlipBasedOnMovement(movementDirection);
        }
        lastPosition = transform.position;
    }

    protected void FlipBasedOnMovement(Vector2 direction)
    {
        if (direction.x < 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (direction.x > 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    public void FacePlayer()
    {
        if (player == null) return;

        if ((player.transform.position.x < transform.position.x && transform.localScale.x < 0) ||
            (player.transform.position.x > transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    protected void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}