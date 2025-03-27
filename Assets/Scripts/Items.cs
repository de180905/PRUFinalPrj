using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] float healAmount = 50f; // Số lượng máu hồi phục

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            // Lấy script HealthSystem của Player
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();

            // Nếu Player có script HealthSystem, gọi hàm hồi máu
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log($"Player collected item, healed for {healAmount}");
                Destroy(gameObject); // Xóa vật phẩm hồi máu sau khi sử dụng
            }
            else
            {
                Debug.LogWarning("HealthSystem not found on Player: " + collision.gameObject.name);
            }
        }
    }
}