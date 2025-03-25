using UnityEngine;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    //thanh mau
    public float maxHealth;
    float currentHealth;
    public Slider healthSlider;

    protected GameObject player;
    protected SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;
    protected Vector3 startPot;
    public bool moveRight = true;
    [SerializeField] public float speed = 1f;
    private Vector3 lastPosition; // Theo dõi vị trí trước đó
    private float minMoveDistance = 0.1f; // Ngưỡng tối thiểu để lật

    protected virtual void Start()
    {
        //thanh mau
        currentHealth = maxHealth;
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPot = transform.position;
        lastPosition = transform.position; // Khởi tạo vị trí ban đầu

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("va cham");
        if(collision.gameObject.tag == "Player")
        {
            AddDamge(1);
        }
    }
    //add damge
    public void AddDamge(float damge)
    {
        currentHealth -= damge;
        healthSlider.value = currentHealth;
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    protected virtual void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= detectionRange)
        {
            OnPlayerDetected();
        }
        else
        {
            Patrol();
        }
        // Tính hướng di chuyển của enemy
        Vector2 movementDirection = (transform.position - lastPosition).normalized;
        if (movementDirection.magnitude > minMoveDistance) // Chỉ lật khi di chuyển đủ xa
        {
            FlipBasedOnMovement(movementDirection);
        }
        lastPosition = transform.position; // Cập nhật vị trí cuối cùng
    }

    
    // Hàm lật dựa trên hướng di chuyển
    protected void FlipBasedOnMovement(Vector2 direction)
    {
        if (direction.x < 0 && transform.localScale.x < 0) // Sang trái, sprite hướng trái -> lật sang phải
        {
            Flip();
        }
        else if (direction.x > 0 && transform.localScale.x > 0) // Sang phải, sprite hướng phải -> lật sang trái
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

    protected virtual void OnPlayerDetected() { }
    protected virtual void Patrol() { }
}
