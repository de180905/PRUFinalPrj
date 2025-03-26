using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float damage = 10f; // Sát thương của đạn

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float xSpeed;
    private bool moveRight; // Hướng di chuyển của đạn

    // Phương thức khởi tạo để xác định hướng bắn
    public void Initialize(float direction)
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        moveRight = direction > 0; // Hướng dựa trên localScale.x của enemy
        xSpeed = bulletSpeed * (moveRight ? 1 : -1);

        rb.freezeRotation = true; // Khóa xoay
        transform.rotation = Quaternion.identity; // Đặt rotation về 0
    }

    void Update()
    {
        // Di chuyển đạn theo trục x
        rb.linearVelocity = new Vector2(xSpeed, 0f);

        // Kiểm tra khoảng cách tối đa
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gây sát thương cho player
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
            }
        }
        // Làm đạn biến mất khi va chạm với bất kỳ thứ gì
        gameObject.SetActive(false);
    }
}