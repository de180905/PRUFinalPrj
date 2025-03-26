using UnityEngine;

public class Items : MonoBehaviour
{
    public float healAmount = 20f; // Số lượng máu hồi phục

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hồi máu");
        if (collision.CompareTag("Player"))
        {
            // Lấy script quản lý máu của Player
            PlayerMovement playerHealth = collision.GetComponent<PlayerMovement>();

            // Nếu Player có script PlayerHealth, gọi hàm hồi máu
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject); // Xóa vật phẩm hồi máu sau khi sử dụng
            }
        }
    }
}
