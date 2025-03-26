using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float damage = 10f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 direction;

    public void Initialize(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on EnemyBullet: " + gameObject.name);
            return;
        }

        startPosition = transform.position;
        this.direction = direction;

        rb.freezeRotation = true;

        // (Optional) Rotate the bullet sprite to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log($"EnemyBullet spawned at {transform.position}, Direction: {direction}, Active: {gameObject.activeSelf}");
    }

    void Update()
    {
        if (rb == null) return;

        rb.linearVelocity = direction * bulletSpeed;

        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            gameObject.SetActive(false);
            Debug.Log($"EnemyBullet {gameObject.name} deactivated due to max distance");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
                Debug.Log($"EnemyBullet hit player and dealt {damage} damage");
            }
            else
            {
                Debug.LogWarning($"HealthSystem not found on player {collision.gameObject.name}");
            }
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        Debug.Log($"EnemyBullet {gameObject.name} collided with {collision.gameObject.name} and deactivated");
    }
}