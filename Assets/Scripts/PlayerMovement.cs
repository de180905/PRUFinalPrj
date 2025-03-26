using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gun;

    // Máu
    [SerializeField] Slider healthSlider;
    [SerializeField] float maxHealth = 100f; // Máu tối đa
    private float currentHealth;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    GameObject currentBullet;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;

        // Cấu hình Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            currentHealth = maxHealth;// Đặt giá trị tối đa của Slider
            healthSlider.value = currentHealth; // Đặt giá trị ban đầu
        }
        else
        {
            Debug.LogError("Health Slider is not assigned in PlayerMovement!");
        }
        if (gun == null)
        {
            Debug.LogError("Gun Transform is not assigned in PlayerMovement!");
        }
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        UpdateHealthSlider();
    }
    // Hàm hồi máu
    public void Heal(float amount)
    {
        currentHealth += amount;
        // Đảm bảo máu không vượt quá giới hạn tối đa
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Máu hiện tại: " + currentHealth);
    }
    // Cập nhật giá trị Slider

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath(); // Gọi khi máu về 0
        }
        Debug.Log($"Player health: {currentHealth}/{maxHealth}");
        UpdateHealthSlider(); // Cập nhật Slider khi nhận sát thương
    }
    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            Debug.Log($"CurrentHealth: {currentHealth}");
            healthSlider.value = currentHealth; // Đồng bộ giá trị Slider với currentHealth
        }
    }
    void OnFire(InputValue value)
    {
        Debug.Log("Fire");
        if (!isAlive) { return; }

        if (currentBullet != null)
        {
            currentBullet.SetActive(false);
        }

        if (gun != null)
        {
            currentBullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
            currentBullet.SetActive(true);

            float direction = transform.localScale.x;
            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(direction);
            }
            else
            {
                Debug.LogError("Bullet script not found on instantiated bullet!");
            }
        }
        else
        {
            Debug.LogWarning("Cannot shoot: Gun Transform is not assigned!");
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        Debug.Log("Jumping");
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("va cham");
        // Va chạm với Enemy dựa trên tag
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                Debug.Log(enemy.GetDamage());
                TakeDamage(enemy.GetDamage()); // Nhận sát thương từ enemy

            }
        }
        // Va chạm với Hazard dựa trên tag

    }

    void Die()
    {

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}