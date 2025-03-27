using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float damage = 25f;

    Rigidbody2D myRigidbody;
    float xSpeed;
    Vector2 startPosition;

    // Phương thức khởi tạo đạn
    public void Initialize(float direction)
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        if (myRigidbody == null)
        {
            Debug.LogError("Rigidbody2D not found on Bullet: " + gameObject.name);
            return;
        }

        startPosition = transform.position;
        xSpeed = direction * bulletSpeed; // Hướng dựa trên direction (localScale.x của player)

        myRigidbody.freezeRotation = true;
        transform.rotation = Quaternion.identity;

        Debug.Log($"Bullet spawned at {transform.position}, Active: {gameObject.activeSelf}, Direction: {direction}");
    }

    void Update()
    {
        if (myRigidbody == null) return;

        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);

        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            gameObject.SetActive(false);
            Debug.Log("Bullet deactivated due to max distance: " + gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
                Debug.Log($"Bullet hit enemy {other.gameObject.name} and dealt {damage} damage");
            }
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        Debug.Log("Bullet collided with " + other.gameObject.name + " and deactivated");
    }
}