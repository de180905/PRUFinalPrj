using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] Slider healthSlider;
    float currentHealth;

    public System.Action OnDeath; // Sự kiện khi chết

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        else
        {
            Debug.LogWarning("Health Slider not found for: " + gameObject.name);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log($"{gameObject.name} took {damage} damage, current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Thêm hàm Heal để hồi máu
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không vượt quá maxHealth
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Cập nhật thanh máu
        }
        Debug.Log($"{gameObject.name} healed for {amount}, current health: {currentHealth}");
    }

    void Die()
    {
        OnDeath?.Invoke(); // Gọi sự kiện chết

        // Kiểm tra xem đối tượng có phải là player không
        if (gameObject.CompareTag("Player"))
        {
            // Tìm GameSession và gọi ProcessPlayerDeath
            GameSession gameSession = FindObjectOfType<GameSession>();
            if (gameSession != null)
            {
                Debug.Log($"{gameObject.name} (Player) died with health: {currentHealth}, calling ProcessPlayerDeath.");
                gameSession.ProcessPlayerDeath();
            }
            else
            {
                Debug.LogWarning("GameSession not found in the scene!");
            }
        }
        else
        {
            // Nếu không phải player (ví dụ: enemy), chỉ tắt GameObject
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} (Enemy) died and was deactivated.");
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}