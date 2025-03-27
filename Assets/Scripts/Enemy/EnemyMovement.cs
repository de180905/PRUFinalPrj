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
        if (player == null)
        {
            Debug.LogWarning($"Player not found by {gameObject.name}");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"SpriteRenderer not found on {gameObject.name}");
        }

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
        if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
        else if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
    }

    public void FacePlayer()
    {
        if (player == null)
        {
            return;
        }

        float directionToPlayer = player.transform.position.x - transform.position.x;
        // Đảo ngược logic để kiểm tra
        if (directionToPlayer < 0 && transform.localScale.x < 0) // Player ở bên trái, nhưng enemy hướng sang trái
        {
            Flip();
        }
        else if (directionToPlayer > 0 && transform.localScale.x > 0) // Player ở bên phải, nhưng enemy hướng sang phải
        {
            Flip();
        }
    }

    protected void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        Debug.Log($"{gameObject.name} flipped, new localScale.x: {scaler.x}");
    }

    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Vector2.Distance(transform.position, player.transform.position);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public bool CanSeePlayer()
    {
        if (player == null)
        {
            return false;
        }

        float distance = GetDistanceToPlayer();
        if (distance > detectionRange) return false;

        return true;
    }
}